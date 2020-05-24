namespace ScribdMpubToEpubConverter
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.table_layout_panel = new System.Windows.Forms.TableLayoutPanel();
            this.listbox_debug_log = new System.Windows.Forms.ListBox();
            this.groupbox_input_data = new System.Windows.Forms.GroupBox();
            this.button_browse_filename_keys = new System.Windows.Forms.Button();
            this.label_filename_keys_xml = new System.Windows.Forms.Label();
            this.textbox_filename_keys_xml = new System.Windows.Forms.TextBox();
            this.groupbox_conversion_options = new System.Windows.Forms.GroupBox();
            this.checkbox_fix_off_by_one_page_references = new System.Windows.Forms.CheckBox();
            this.checkbox_generate_cover_page = new System.Windows.Forms.CheckBox();
            this.checkbox_show_messagebox_upon_completion = new System.Windows.Forms.CheckBox();
            this.checkbox_enable_decryption = new System.Windows.Forms.CheckBox();
            this.textbox_folder_path = new System.Windows.Forms.TextBox();
            this.button_browse_dialog = new System.Windows.Forms.Button();
            this.button_convert = new System.Windows.Forms.Button();
            this.groupbox_logging_options = new System.Windows.Forms.GroupBox();
            this.label_log_level = new System.Windows.Forms.Label();
            this.combobox_log_level = new System.Windows.Forms.ComboBox();
            this.table_layout_panel.SuspendLayout();
            this.groupbox_input_data.SuspendLayout();
            this.groupbox_conversion_options.SuspendLayout();
            this.groupbox_logging_options.SuspendLayout();
            this.SuspendLayout();
            // 
            // table_layout_panel
            // 
            this.table_layout_panel.ColumnCount = 4;
            this.table_layout_panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.table_layout_panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.table_layout_panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.table_layout_panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.table_layout_panel.Controls.Add(this.listbox_debug_log, 0, 0);
            this.table_layout_panel.Controls.Add(this.groupbox_input_data, 0, 1);
            this.table_layout_panel.Controls.Add(this.groupbox_conversion_options, 0, 2);
            this.table_layout_panel.Controls.Add(this.textbox_folder_path, 0, 3);
            this.table_layout_panel.Controls.Add(this.button_browse_dialog, 3, 3);
            this.table_layout_panel.Controls.Add(this.button_convert, 0, 4);
            this.table_layout_panel.Controls.Add(this.groupbox_logging_options, 2, 2);
            this.table_layout_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table_layout_panel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.table_layout_panel.Location = new System.Drawing.Point(0, 0);
            this.table_layout_panel.Margin = new System.Windows.Forms.Padding(0);
            this.table_layout_panel.MinimumSize = new System.Drawing.Size(500, 510);
            this.table_layout_panel.Name = "table_layout_panel";
            this.table_layout_panel.RowCount = 5;
            this.table_layout_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.table_layout_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.5F));
            this.table_layout_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 32.5F));
            this.table_layout_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.table_layout_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.table_layout_panel.Size = new System.Drawing.Size(549, 511);
            this.table_layout_panel.TabIndex = 0;
            // 
            // listbox_debug_log
            // 
            this.table_layout_panel.SetColumnSpan(this.listbox_debug_log, 4);
            this.listbox_debug_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listbox_debug_log.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listbox_debug_log.FormattingEnabled = true;
            this.listbox_debug_log.HorizontalScrollbar = true;
            this.listbox_debug_log.ItemHeight = 15;
            this.listbox_debug_log.Location = new System.Drawing.Point(5, 5);
            this.listbox_debug_log.Margin = new System.Windows.Forms.Padding(5);
            this.listbox_debug_log.Name = "listbox_debug_log";
            this.listbox_debug_log.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listbox_debug_log.Size = new System.Drawing.Size(539, 194);
            this.listbox_debug_log.TabIndex = 0;
            // 
            // groupbox_input_data
            // 
            this.table_layout_panel.SetColumnSpan(this.groupbox_input_data, 4);
            this.groupbox_input_data.Controls.Add(this.button_browse_filename_keys);
            this.groupbox_input_data.Controls.Add(this.label_filename_keys_xml);
            this.groupbox_input_data.Controls.Add(this.textbox_filename_keys_xml);
            this.groupbox_input_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupbox_input_data.Location = new System.Drawing.Point(5, 204);
            this.groupbox_input_data.Margin = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.groupbox_input_data.Name = "groupbox_input_data";
            this.groupbox_input_data.Padding = new System.Windows.Forms.Padding(0);
            this.groupbox_input_data.Size = new System.Drawing.Size(539, 84);
            this.groupbox_input_data.TabIndex = 1;
            this.groupbox_input_data.TabStop = false;
            this.groupbox_input_data.Text = "Input data";
            // 
            // button_browse_filename_keys
            // 
            this.button_browse_filename_keys.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_browse_filename_keys.Location = new System.Drawing.Point(5, 60);
            this.button_browse_filename_keys.Margin = new System.Windows.Forms.Padding(5);
            this.button_browse_filename_keys.MaximumSize = new System.Drawing.Size(0, 25);
            this.button_browse_filename_keys.MinimumSize = new System.Drawing.Size(525, 20);
            this.button_browse_filename_keys.Name = "button_browse_filename_keys";
            this.button_browse_filename_keys.Size = new System.Drawing.Size(525, 20);
            this.button_browse_filename_keys.TabIndex = 2;
            this.button_browse_filename_keys.Text = "...";
            this.button_browse_filename_keys.UseVisualStyleBackColor = true;
            this.button_browse_filename_keys.Click += new System.EventHandler(this.OnButtonBrowseFilenameKeysClick);
            // 
            // label_filename_keys_xml
            // 
            this.label_filename_keys_xml.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_filename_keys_xml.AutoSize = true;
            this.label_filename_keys_xml.Location = new System.Drawing.Point(5, 15);
            this.label_filename_keys_xml.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_filename_keys_xml.Name = "label_filename_keys_xml";
            this.label_filename_keys_xml.Size = new System.Drawing.Size(136, 13);
            this.label_filename_keys_xml.TabIndex = 1;
            this.label_filename_keys_xml.Text = "FILENAME_KEYS.xml path";
            // 
            // textbox_filename_keys_xml
            // 
            this.textbox_filename_keys_xml.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_filename_keys_xml.Location = new System.Drawing.Point(5, 35);
            this.textbox_filename_keys_xml.Margin = new System.Windows.Forms.Padding(5);
            this.textbox_filename_keys_xml.MaxLength = 32;
            this.textbox_filename_keys_xml.MinimumSize = new System.Drawing.Size(4, 20);
            this.textbox_filename_keys_xml.Name = "textbox_filename_keys_xml";
            this.textbox_filename_keys_xml.Size = new System.Drawing.Size(526, 20);
            this.textbox_filename_keys_xml.TabIndex = 0;
            // 
            // groupbox_conversion_options
            // 
            this.table_layout_panel.SetColumnSpan(this.groupbox_conversion_options, 2);
            this.groupbox_conversion_options.Controls.Add(this.checkbox_fix_off_by_one_page_references);
            this.groupbox_conversion_options.Controls.Add(this.checkbox_generate_cover_page);
            this.groupbox_conversion_options.Controls.Add(this.checkbox_show_messagebox_upon_completion);
            this.groupbox_conversion_options.Controls.Add(this.checkbox_enable_decryption);
            this.groupbox_conversion_options.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupbox_conversion_options.Location = new System.Drawing.Point(5, 293);
            this.groupbox_conversion_options.Margin = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.groupbox_conversion_options.Name = "groupbox_conversion_options";
            this.groupbox_conversion_options.Padding = new System.Windows.Forms.Padding(0);
            this.groupbox_conversion_options.Size = new System.Drawing.Size(263, 161);
            this.groupbox_conversion_options.TabIndex = 2;
            this.groupbox_conversion_options.TabStop = false;
            this.groupbox_conversion_options.Text = "Conversion options";
            // 
            // checkbox_fix_off_by_one_page_references
            // 
            this.checkbox_fix_off_by_one_page_references.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkbox_fix_off_by_one_page_references.AutoSize = true;
            this.checkbox_fix_off_by_one_page_references.Checked = true;
            this.checkbox_fix_off_by_one_page_references.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkbox_fix_off_by_one_page_references.Location = new System.Drawing.Point(10, 80);
            this.checkbox_fix_off_by_one_page_references.Margin = new System.Windows.Forms.Padding(5);
            this.checkbox_fix_off_by_one_page_references.Name = "checkbox_fix_off_by_one_page_references";
            this.checkbox_fix_off_by_one_page_references.Size = new System.Drawing.Size(217, 17);
            this.checkbox_fix_off_by_one_page_references.TabIndex = 3;
            this.checkbox_fix_off_by_one_page_references.Text = "Attempt to fix off-by-one page references";
            this.checkbox_fix_off_by_one_page_references.UseVisualStyleBackColor = true;
            // 
            // checkbox_generate_cover_page
            // 
            this.checkbox_generate_cover_page.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkbox_generate_cover_page.AutoSize = true;
            this.checkbox_generate_cover_page.Location = new System.Drawing.Point(10, 40);
            this.checkbox_generate_cover_page.Margin = new System.Windows.Forms.Padding(10, 20, 20, 10);
            this.checkbox_generate_cover_page.Name = "checkbox_generate_cover_page";
            this.checkbox_generate_cover_page.Size = new System.Drawing.Size(127, 17);
            this.checkbox_generate_cover_page.TabIndex = 2;
            this.checkbox_generate_cover_page.Text = "Generate cover page";
            this.checkbox_generate_cover_page.UseVisualStyleBackColor = true;
            // 
            // checkbox_show_messagebox_upon_completion
            // 
            this.checkbox_show_messagebox_upon_completion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkbox_show_messagebox_upon_completion.AutoSize = true;
            this.checkbox_show_messagebox_upon_completion.Checked = true;
            this.checkbox_show_messagebox_upon_completion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkbox_show_messagebox_upon_completion.Location = new System.Drawing.Point(10, 60);
            this.checkbox_show_messagebox_upon_completion.Margin = new System.Windows.Forms.Padding(5);
            this.checkbox_show_messagebox_upon_completion.Name = "checkbox_show_messagebox_upon_completion";
            this.checkbox_show_messagebox_upon_completion.Size = new System.Drawing.Size(196, 17);
            this.checkbox_show_messagebox_upon_completion.TabIndex = 1;
            this.checkbox_show_messagebox_upon_completion.Text = "Show messagebox upon completion";
            this.checkbox_show_messagebox_upon_completion.UseVisualStyleBackColor = true;
            // 
            // checkbox_enable_decryption
            // 
            this.checkbox_enable_decryption.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkbox_enable_decryption.AutoSize = true;
            this.checkbox_enable_decryption.Checked = true;
            this.checkbox_enable_decryption.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkbox_enable_decryption.Location = new System.Drawing.Point(10, 20);
            this.checkbox_enable_decryption.Margin = new System.Windows.Forms.Padding(10, 20, 20, 10);
            this.checkbox_enable_decryption.Name = "checkbox_enable_decryption";
            this.checkbox_enable_decryption.Size = new System.Drawing.Size(111, 17);
            this.checkbox_enable_decryption.TabIndex = 0;
            this.checkbox_enable_decryption.Text = "Enable decryption";
            this.checkbox_enable_decryption.UseVisualStyleBackColor = true;
            // 
            // textbox_folder_path
            // 
            this.table_layout_panel.SetColumnSpan(this.textbox_folder_path, 3);
            this.textbox_folder_path.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textbox_folder_path.Enabled = false;
            this.textbox_folder_path.Location = new System.Drawing.Point(5, 464);
            this.textbox_folder_path.Margin = new System.Windows.Forms.Padding(5);
            this.textbox_folder_path.MinimumSize = new System.Drawing.Size(250, 20);
            this.textbox_folder_path.Name = "textbox_folder_path";
            this.textbox_folder_path.Size = new System.Drawing.Size(482, 20);
            this.textbox_folder_path.TabIndex = 3;
            // 
            // button_browse_dialog
            // 
            this.button_browse_dialog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_browse_dialog.Enabled = false;
            this.button_browse_dialog.Location = new System.Drawing.Point(497, 464);
            this.button_browse_dialog.Margin = new System.Windows.Forms.Padding(5);
            this.button_browse_dialog.MaximumSize = new System.Drawing.Size(0, 20);
            this.button_browse_dialog.MinimumSize = new System.Drawing.Size(0, 20);
            this.button_browse_dialog.Name = "button_browse_dialog";
            this.button_browse_dialog.Size = new System.Drawing.Size(47, 20);
            this.button_browse_dialog.TabIndex = 4;
            this.button_browse_dialog.Text = "&...";
            this.button_browse_dialog.UseVisualStyleBackColor = true;
            this.button_browse_dialog.Click += new System.EventHandler(this.OnButtonBrowseDialogClick);
            // 
            // button_convert
            // 
            this.table_layout_panel.SetColumnSpan(this.button_convert, 4);
            this.button_convert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_convert.Enabled = false;
            this.button_convert.Location = new System.Drawing.Point(5, 489);
            this.button_convert.Margin = new System.Windows.Forms.Padding(5);
            this.button_convert.MaximumSize = new System.Drawing.Size(0, 25);
            this.button_convert.MinimumSize = new System.Drawing.Size(0, 20);
            this.button_convert.Name = "button_convert";
            this.button_convert.Size = new System.Drawing.Size(539, 20);
            this.button_convert.TabIndex = 5;
            this.button_convert.Text = "&Convert";
            this.button_convert.UseVisualStyleBackColor = true;
            this.button_convert.Click += new System.EventHandler(this.OnButtonConvertClick);
            // 
            // groupbox_logging_options
            // 
            this.table_layout_panel.SetColumnSpan(this.groupbox_logging_options, 2);
            this.groupbox_logging_options.Controls.Add(this.label_log_level);
            this.groupbox_logging_options.Controls.Add(this.combobox_log_level);
            this.groupbox_logging_options.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupbox_logging_options.Location = new System.Drawing.Point(278, 293);
            this.groupbox_logging_options.Margin = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.groupbox_logging_options.Name = "groupbox_logging_options";
            this.groupbox_logging_options.Padding = new System.Windows.Forms.Padding(5);
            this.groupbox_logging_options.Size = new System.Drawing.Size(266, 161);
            this.groupbox_logging_options.TabIndex = 6;
            this.groupbox_logging_options.TabStop = false;
            this.groupbox_logging_options.Text = "Logging options";
            // 
            // label_log_level
            // 
            this.label_log_level.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_log_level.AutoSize = true;
            this.label_log_level.Location = new System.Drawing.Point(5, 15);
            this.label_log_level.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_log_level.Name = "label_log_level";
            this.label_log_level.Size = new System.Drawing.Size(50, 13);
            this.label_log_level.TabIndex = 3;
            this.label_log_level.Text = "Log level";
            // 
            // combobox_log_level
            // 
            this.combobox_log_level.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.combobox_log_level.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combobox_log_level.FormattingEnabled = true;
            this.combobox_log_level.Location = new System.Drawing.Point(5, 35);
            this.combobox_log_level.Name = "combobox_log_level";
            this.combobox_log_level.Size = new System.Drawing.Size(255, 21);
            this.combobox_log_level.TabIndex = 0;
            this.combobox_log_level.SelectedIndexChanged += new System.EventHandler(this.OnComboboxLogLevelSelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 511);
            this.Controls.Add(this.table_layout_panel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(565, 550);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SCRIMTEC - SCRIbd Mpub To Epub Converter";
            this.table_layout_panel.ResumeLayout(false);
            this.table_layout_panel.PerformLayout();
            this.groupbox_input_data.ResumeLayout(false);
            this.groupbox_input_data.PerformLayout();
            this.groupbox_conversion_options.ResumeLayout(false);
            this.groupbox_conversion_options.PerformLayout();
            this.groupbox_logging_options.ResumeLayout(false);
            this.groupbox_logging_options.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel table_layout_panel;
		private System.Windows.Forms.ListBox listbox_debug_log;
		private System.Windows.Forms.GroupBox groupbox_input_data;
		private System.Windows.Forms.TextBox textbox_folder_path;
		private System.Windows.Forms.Button button_browse_dialog;
		private System.Windows.Forms.Button button_convert;
		private System.Windows.Forms.Button button_browse_filename_keys;
		private System.Windows.Forms.Label label_filename_keys_xml;
		private System.Windows.Forms.TextBox textbox_filename_keys_xml;
		private System.Windows.Forms.GroupBox groupbox_conversion_options;
		private System.Windows.Forms.CheckBox checkbox_enable_decryption;
		private System.Windows.Forms.GroupBox groupbox_logging_options;
		private System.Windows.Forms.Label label_log_level;
		private System.Windows.Forms.ComboBox combobox_log_level;
		private System.Windows.Forms.CheckBox checkbox_show_messagebox_upon_completion;
		private System.Windows.Forms.CheckBox checkbox_generate_cover_page;
		private System.Windows.Forms.CheckBox checkbox_fix_off_by_one_page_references;
	}
}

