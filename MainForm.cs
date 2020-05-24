using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ScribdMpubToEpubConverter
{
	public partial class MainForm : Form
	{
		private readonly Scribd.Decryptor Decryptor = new Scribd.Decryptor();
		private Dictionary<string, string> DecryptionKeys = new Dictionary<string, string>();

		public MainForm()
		{
			InitializeComponent();

			// Overwrite debug log buffer
			listbox_debug_log.DataSource = Helper.DebugLog;

			// Add logger levels
			combobox_log_level.Items.AddRange(Logger.Levels);

			// Set logger level
			combobox_log_level.SelectedIndex = Logger.CurrentLevel;

			// Set debug log path
			Helper.DebugLogPath = Directory.GetCurrentDirectory() +
				Path.DirectorySeparatorChar +
				"debug_log.txt";

			// Delete previous debug log file
			File.Delete(Helper.DebugLogPath);

			Helper.Information("Logging session to " + Helper.DebugLogPath);
			Helper.Information("---------------------------------");
			Helper.Information("https://github.com/BELGRADE-OUTLAW/SCRIMTEC");
		}

		private void ToggleInputElements(bool state)
		{
			button_browse_dialog.Enabled = textbox_folder_path.Enabled = state;
		}

		private void DecryptDirectories()
		{
			var used_keys = new Dictionary<string, string>();

			var subdirectory_list = Directory.GetDirectories(textbox_folder_path.Text);
			foreach (var subdirectory in subdirectory_list)
			{
				var subdirectory_name = Path.GetFileName(subdirectory);
				foreach (var decryption_key in DecryptionKeys)
				{
					if (subdirectory_name != decryption_key.Key)
						continue;

					Helper.Debug("Decrypting directory: " + subdirectory);

					Decryptor.GeneratePrivateKey(decryption_key.Value);
					Decryptor.Decrypt(subdirectory);

					used_keys.Add(decryption_key.Key, decryption_key.Value);
				}
			}

			var unused_keys = DecryptionKeys.Except(used_keys);
			if (unused_keys.Count() != 0)
			{
				Helper.Information("Unused decryption keys: " + unused_keys.Count());
				foreach (var decryption_key in unused_keys)
				{
					Helper.Information(
						"- " + decryption_key.Key +
						" with value: " + decryption_key.Value
					);
				}
			}
		}

		private void ConvertDirectoryToEPUB(string subdirectory)
		{
			Helper.Debug("Converting directory: " + subdirectory);

			// Parse book content
			var book = new Scribd.Book(subdirectory);

			// Convert parsed content to EPUB
			var book_converter = new Scribd.BookConverter(
				book,
				checkbox_generate_cover_page.Checked,
				checkbox_fix_off_by_one_page_references.Checked
			);

			book_converter.Convert();

			// Generate and save EPUB
			var epub = new EPUB2.BookGenerator(book);
			epub.Generate();
		}

		private void ConvertDirectories()
		{
			var subdirectory_list = Directory.GetDirectories(textbox_folder_path.Text);
			foreach (var subdirectory in subdirectory_list)
			{
				var subdirectory_name = Path.GetFileName(subdirectory);
				foreach (var decryption_key in DecryptionKeys)
				{
					if (subdirectory_name != decryption_key.Key)
						continue;

					// Every book must have book_metadata.json
					var is_book = File.Exists(
						subdirectory + Path.DirectorySeparatorChar + "book_metadata.json"
					);

					if (!is_book)
						continue;

					ConvertDirectoryToEPUB(subdirectory);
				}
			}
		}

		private void OnButtonConvertClick(object sender, EventArgs e)
		{
			if (textbox_folder_path.Text.Length == 0)
			{
				Helper.ShowWarning("Selected directory path is empty!");
				return;
			}

			if (!Directory.Exists(textbox_folder_path.Text))
			{
				Helper.ShowWarning("Selected directory doesn't exist!");
				return;
			}

			// Reset warning count to see how many warnings happened during
			// decryption/conversion/both and finally inform the user if they did.
			Helper.WarningCount = 0;

			try
			{
				if (checkbox_enable_decryption.Checked)
					DecryptDirectories();

				ConvertDirectories();

				switch (Helper.WarningCount)
				{
					case 0:
						if (checkbox_show_messagebox_upon_completion.Checked)
						{
							MessageBox.Show(
								"The conversion job has been finished.",
								"Information",
								MessageBoxButtons.OK,
								MessageBoxIcon.Information
							);
						}
						break;
					default:
						var message = "The conversion job has encountered " +
							Helper.WarningCount + " warning" +
							(Helper.WarningCount != 1 ? "s" : "") +
							" during conversion!";
						Helper.ShowWarning(message + "\nPlease check the debug log for details.");
						Helper.Warning(message);
						break;
				}
			}
			catch (Exception exception)
			{
				var message = exception.Message + "\n" +
					exception.StackTrace + "\n" +
					exception.Source + "\n" +
					exception.ToString() + "\n";

				Helper.ShowWarning(message, "An exception was thrown!");
				Helper.Warning(message);
			}
		}

		private void OnButtonBrowseFilenameKeysClick(object sender, EventArgs e)
		{
			try
			{
				using var dialog = new OpenFileDialog
				{
					InitialDirectory = Directory.GetCurrentDirectory(),
					RestoreDirectory = true,
					Multiselect = false,
					Filter = "Scribd decryption key list|FILENAME_KEYS.xml"
				};

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					textbox_filename_keys_xml.Text = dialog.FileName;

					// Remove all decryption keys, just in case
					if (DecryptionKeys.Count != 0)
					{
						DecryptionKeys.Clear();
						Helper.Debug("Cleaning up leftover decryption keys");
					}

					DecryptionKeyListParser.Parse(
						dialog.FileName,
						ref DecryptionKeys
					);

					ToggleInputElements(true);
				}
			}
			catch (Exception exception)
			{
				var message = exception.Message + "\n" +
					exception.StackTrace + "\n" +
					exception.Source + "\n" +
					exception.ToString() + "\n";

				Helper.ShowWarning(message, "An exception was thrown!");
				Helper.Warning(message);
			}
		}

		private void OnButtonBrowseDialogClick(object sender, EventArgs e)
		{
			using var dialog = new FolderBrowserDialog
			{
				Description = "Select your document_cache directory",
				SelectedPath = Directory.GetCurrentDirectory()
			};

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				var directory_name = Path.GetFileName(dialog.SelectedPath);
				if (directory_name != "document_cache")
				{
					Helper.ShowWarning("Please select your document_cache directory!");
					OnButtonBrowseDialogClick(sender, e);
					return;
				}

				textbox_folder_path.Text = dialog.SelectedPath;
				button_convert.Enabled = true;
			}
		}

		private void OnComboboxLogLevelSelectedIndexChanged(object sender, EventArgs e)
		{
			Logger.CurrentLevel = (sender as ComboBox).SelectedIndex;
		}
	}
}
