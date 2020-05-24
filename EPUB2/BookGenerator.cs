using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace ScribdMpubToEpubConverter.EPUB2
{
	class BookGenerator
	{
		private Scribd.Book Book { get; set; }
		private string CurrentDirectory { get; set; }
		private string CurrentBookName { get; set; }

		private readonly NcxDocument Ncx = new NcxDocument();
		private readonly OpfDocument Opf = new OpfDocument();
		private readonly OcfDocument Ocf = new OcfDocument();

		public BookGenerator(Scribd.Book book)
		{
			Book = book;

			// Set current book name in the format of: "Title - Book ID"
			CurrentBookName = Book.Title + " - " + Book.ID.ToString();

			// Get Windows filesystem-compliant directory name
			var invalid_characters = Path.GetInvalidFileNameChars();

			// Sanitize directory name
			CurrentBookName = string.Join(
				"_",
				CurrentBookName.Split(
					invalid_characters,
					System.StringSplitOptions.RemoveEmptyEntries
				)
			).TrimEnd('.');

			// Delete directory if it already exists
			if (Directory.Exists(CurrentBookName))
				DeleteDirectory(CurrentBookName);

			CurrentDirectory = Helper.CreateDirectory(CurrentBookName);

			// Set document titles
			Opf.Title = Ncx.Title = Book.Title;

			// Create required directories
			foreach (var subdirectory in EpubDirectories.Directories)
				Directory.CreateDirectory(CurrentDirectory + subdirectory);

			// Generate stylesheet file
			GenerateStylesheet();
		}

		private void GenerateStylesheet()
		{
			Helper.Debug("Generating stylesheet");

			string css = string.Empty;
			foreach (var style in Book.Styles)
			{
				if (style.Value.Trim().Length == 0)
					continue;

				css += "." + style.Key + "{ " + style.Value + "; }\r\n";
			}

			if (css.Trim().Length == 0)
				Helper.Warning("CSS is empty! Please delete and unlink from all HTML files!");

			File.WriteAllText(
				CurrentDirectory + EpubDirectories.OEBPS_Styles + "style.css",
				css
			);
		}

		private void AddMimetype(ZipStorer zip)
		{
			Helper.Debug("Adding mimetype");

			// Make sure there is no UTF-8 BOM
			var encoding = new UTF8Encoding(false);
			var mimetype = encoding.GetBytes("application/epub+zip");

			File.WriteAllBytes(CurrentDirectory + "mimetype", mimetype);

			// Add mimetype without compression
			zip.AddFile(
				ZipStorer.Compression.Store,
				CurrentDirectory + "mimetype",
				"mimetype"
			);
		}

		private void AddCoverPage()
		{
			if (Book.CoverPage.HTML == null)
				return;

			Helper.Debug("Adding cover page");

			var html_page_name = "coverpage";

			// NOTE: Relative to the EPUB root
			var epub_relative_path = EpubDirectories.OEBPS_Text + html_page_name + ".xhtml";
			var absolute_path = CurrentDirectory + epub_relative_path;

			// Write HTML to disk
			File.WriteAllText(absolute_path, Book.CoverPage.HTML);

			// NOTE: Relative to the OEBPS/ folder
			var oebps_relative_path = epub_relative_path.Replace(EpubDirectories.OEBPS, "");
			Ncx.AddNavigationItem(html_page_name, Book.CoverPage.Title, oebps_relative_path);
			Opf.AddManifestItem(html_page_name, oebps_relative_path);

			// NOTE: Path relative to the OEBPS/ folder
			var toc_relative_path = Ncx.Type.GetRelativePath().Replace(EpubDirectories.OEBPS, "");
			Opf.AddManifestItem("ncx", toc_relative_path);
		}

		private void AddChapters()
		{
			Helper.Debug("Adding chapters and linking NCX");

			foreach (var chapter in Book.Chapters)
			{
				var html_page_id = Path.GetFileNameWithoutExtension(chapter.OutputFileName);

				// NOTE: Relative to the EPUB root
				var epub_relative_path = EpubDirectories.OEBPS_Text + chapter.OutputFileName;
				var absolute_path = CurrentDirectory + epub_relative_path;

				// Write HTML to disk
				File.WriteAllText(absolute_path, chapter.HTML);

				// NOTE: Relative to the OEBPS/ folder
				var oebps_relative_path = epub_relative_path.Replace(EpubDirectories.OEBPS, "");
				Ncx.AddNavigationItem(html_page_id, chapter.Title, oebps_relative_path);
				Opf.AddManifestItem(html_page_id, oebps_relative_path);
			}

			// NOTE: Path relative to the OEBPS/ folder
			var toc_relative_path = Ncx.Type.GetRelativePath().Replace(EpubDirectories.OEBPS, "");
			Opf.AddManifestItem("ncx", toc_relative_path);
		}

		private void AddImages()
		{
			Helper.Debug("Adding images");

			foreach (var image in Book.Images)
			{
				var epub_relative_path = EpubDirectories.OEBPS_Images + image.Key;
				File.Copy(image.Value.AbsoluteFilePath, CurrentDirectory + epub_relative_path);

				// NOTE: Path relative to the OEBPS/ folder
				var image_relative_path = epub_relative_path.Replace(EpubDirectories.OEBPS, "");
				Opf.AddManifestItem(image.Value.ID, image_relative_path);
			}
		}

		private void AddStylesheet()
		{
			Helper.Debug("Adding stylesheet");

			var epub_relative_path = EpubDirectories.OEBPS_Styles + "style.css";

			// NOTE: Path relative to the OEBPS/ folder
			var css_relative_path = epub_relative_path.Replace(EpubDirectories.OEBPS, "");
			Opf.AddManifestItem("css", css_relative_path);
		}

		// NOTE: When handling with the ZipStorer class, please use
		// \ as a directory separator instead of /
		// Otherwise, you are free to use, but the rule of thumb is:
		// - / as a separator for EPUB stuff
		// - \ as a separator for Windows filesystem stuff
		public void Generate()
		{
			var zip = ZipStorer.Create(
				Path.GetDirectoryName(CurrentDirectory) + ".epub"
			);

			AddMimetype(zip);
			AddCoverPage();
			AddChapters();
			AddImages();
			AddStylesheet();

			var Documents = new Dictionary<DocumentType, IEpubDocument>{
				{ DocumentType.NCX, Ncx },
				{ DocumentType.OPF, Opf },
				{ DocumentType.OCF, Ocf }
			};

			foreach (var document in Documents)
			{
				var absolute_path = CurrentDirectory + document.Key.GetRelativePath();

				using var stream = File.OpenWrite(absolute_path);
				document.Value.Write(stream);

				Helper.Debug("Generating " + document.Key.GetRelativePath());
			}

			// Add META-INF/ folder
			var meta_inf = EpubDirectories.METAINF.Replace('/', '\\');
			zip.AddDirectory(
				ZipStorer.Compression.Deflate,
				CurrentDirectory + meta_inf,
				meta_inf
			);

			// Add OEBPS/ folder
			var oebps = EpubDirectories.OEBPS.Replace('/', '\\');
			zip.AddDirectory(
				ZipStorer.Compression.Deflate,
				CurrentDirectory + oebps,
				oebps
			);

			zip.Close();

			// Delete temporary files
			DeleteDirectory(CurrentDirectory);

			Helper.Debug("Removed temporary files from the filesystem");
			Helper.Information("Finished generating an EPUB2.0.1 file for \"" + CurrentBookName + "\"");
			Helper.Debug("---------------------------------");
		}

		public static void DeleteDirectory(string path)
		{
			foreach (string directory in Directory.GetDirectories(path))
				DeleteDirectory(directory);

			try
			{
				Directory.Delete(path, true);
			}
			catch (IOException)
			{
				Directory.Delete(path, true);
			}
			catch (System.UnauthorizedAccessException)
			{
				Directory.Delete(path, true);
			}
		}
	}
}