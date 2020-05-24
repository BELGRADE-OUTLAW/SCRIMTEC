using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ScribdMpubToEpubConverter.Scribd
{
	struct HtmlPageInfo
	{
		public Chapter Chapter { get; set; }
		public HtmlGenerator Html { get; set; }

		// Indicates whether we're currently populating a table
		public bool InTable { get; set; }

		// Prevent spamming of a warning about a style property with a -scribd* substring
		// Indicates that the warning has already been displayed for this chapter
		public bool WarnedUnsupportedStyle { get; set; }
	}

	class BookConverter
	{
		private Book Book { get; set; }
		private bool ShouldGenerateCoverPage { get; set; }
		private bool ShouldFixOffByOnePageReferences { get; set; }

		private HtmlPageInfo CurrentPage;

		public BookConverter(
			Book book,
			bool should_generate_cover_page,
			bool should_fix_off_by_one_page_references
		)
		{
			Book = book;
			ShouldGenerateCoverPage = should_generate_cover_page;
			ShouldFixOffByOnePageReferences = should_fix_off_by_one_page_references;
		}

		private static bool IsCorruptImage(byte[] content)
		{
			try
			{
				using var stream = new MemoryStream(content);
				using var bitmap = new System.Drawing.Bitmap(stream);
				return false;
			}
			catch
			{
				return true;
			}
		}

		private bool AcquireCoverPage()
		{
			// Find the corresponding chapter
			// NOTE: Logically, one would assume that the cover should
			// be found when the chapter type is "frontmatter", but not every
			// book has a chapter as such. Therefore, we are going to use the first
			// image we find, as the cover image.
			for (int i = 0; i < Book.Chapters.Length; ++i)
			{
				// Find the first image
				foreach (var block in Book.Chapters[i].Content.Blocks)
				{
					if (block.Type != "image")
						continue;

					Helper.Debug("Adding cover page");

					// Obtain image path and filename
					var image_path = Book.Directory +
						Path.DirectorySeparatorChar +
						Book.Chapters[i].FilePath +
						Path.DirectorySeparatorChar +
						block.Src;

					var image_name = Path.GetFileName(image_path);
					var image_content = File.ReadAllBytes(image_path);

					// Shouldn't happen
					if (IsCorruptImage(image_content))
						Helper.Warning("You should redownload the book. Found corrupt image: " + image_name);

					Helper.Debug("Adding image as cover: " + image_name);

					Book.Images.Add(image_name, new HtmlImage
					{
						AbsoluteFilePath = image_path,
						Content = image_content,
						ID = "cover"
					});

					using var string_writer = new UTF8StringWriter();
					using var writer = XmlWriter.Create(string_writer, Helper.XmlWriterSettings);

					var title = "Cover Page";
					var html = new HtmlGenerator(writer, title);
					html.AddImage(image_name, block.Alt ?? image_name, true);

					writer.Close();

					// Only HTML and Title fields are populated for the cover page
					Book.CoverPage = new Chapter
					{
						HTML = string_writer.ToString(),
						Title = title
					};

					// We found the cover, return from function
					return true;
				}
			}

			return false;
		}

		private string HandleWordContent(WordContent word_content)
		{
			var word_text = string.Empty;
			switch (word_content.Type)
			{
				case "composite":
					foreach (var word in word_content.Words)
						word_text += HandleWordContent(word);
					break;
				case "image":
					Helper.Warning(
						"Found image inside a word block! Ignoring..."
					);
					break;
				default:
					word_text += word_content.Text;
					break;
			}

			return word_text;
		}

		// TODO: Parse arrays?
		private HtmlAttribute GetClassAttribute(dynamic style)
		{
			// The same logic applies, just as with metadata.
			// NOTE: This abuses an underlying hack in HtmlGenerator.AddText()!
			// Specifically, when LocalName is set to null, then the attribute won't be added.
			// Meaning, that the attribute will only be added if the if cases underneath succeed.
			HtmlAttribute class_attribute = new HtmlAttribute();
			if (style != null)
			{
				var style_name = style as string;
				if (style_name == null)
					return class_attribute;

				// Include the style, only if it was successfully added (i.e. not empty)
				if (Book.Styles.ContainsKey(style_name))
				{
					class_attribute.LocalName = "class";
					class_attribute.Value = style;
				}
			}

			return class_attribute;
		}

		private HtmlAttribute GetStyleAttribute(BlockContent block)
		{
			// The same logic applies, just as with metadata.
			// NOTE: This abuses an underlying hack in HtmlGenerator.AddText()!
			// Specifically, when LocalName is set to null, then the attribute won't be added.
			// Meaning, that the attribute will only be added if the if cases underneath succeed.
			HtmlAttribute style_attribute = new HtmlAttribute();

			if (block.Align != null)
			{
				var align_type = block.Align as string;
				if (align_type == null)
					return style_attribute;

				style_attribute.LocalName = "style";
				style_attribute.Value = "text-align: " + align_type;
			}

			return style_attribute;
		}

		private bool GetProcessedLink(MetadataContent metadata, ref string link)
		{
			int find_chapter_from_block_index(int block_index)
			{
				for (int i = 0; i < Book.TableOfContents.Length; ++i)
				{
					var toc_info = Book.TableOfContents[i];
					if (block_index >= toc_info.BlockStart && block_index < toc_info.BlockEnd)
					{
						// Fix an edge case where some books have a TOC
						// that links to the end of the previous chapter
						if (ShouldFixOffByOnePageReferences)
						{
							// Check if the block is within the chapter
							// If it is, then return the next chapter
							if (block_index + 1 >= toc_info.BlockEnd)
							{
								Helper.Information(
									"Fixing probable off-by-one reference from chapter " + i + " to chapter " + (i + 1)
								);
								return i + 1;
							}
						}
						return i;
					}
				}

				return -1;
			}

			int find_block_index_from_link(string link)
			{
				try
				{
					return Book.Tags[link].BlockIndex;
				}
				catch
				{
					return -1;
				}
			}

			// The initial assignment is a fallback
			var is_external_link = (metadata.Href.StartsWith("http") || metadata.Href.StartsWith("www"));
			if (metadata.ExternalLink != null)
				is_external_link = metadata.ExternalLink.Value;

			Helper.Debug(
				"Adding an " + (is_external_link ? "external" : "internal") + " link: " + metadata.Href
			);

			// Don't process external links
			if (is_external_link)
			{
				link = metadata.Href;
				return true;
			}

			var block_index = find_block_index_from_link(metadata.Href);
			if (block_index == -1)
			{
				Helper.Warning("Failed to find block index for given link: " + metadata.Href);
				return false;
			}

			var chapter_index = find_chapter_from_block_index(block_index);
			if (chapter_index == -1)
			{
				Helper.Warning("Failed to find chapter index for given block index: " + block_index);
				return false;
			}

			// Link to appropriate chapter
			link = Book.Chapters[chapter_index].OutputFileName;
			return true;
		}

		// TODO: Parse arrays?
		private HtmlAttribute GetHrefAttribute(dynamic metadata)
		{
			// NOTE: This abuses an underlying hack in HtmlGenerator.AddText()!
			// Specifically, when LocalName is set to null, then the attribute won't be added.
			// Meaning, that the attribute will only be added if the if cases underneath succeed.
			HtmlAttribute href_attribute = new HtmlAttribute();

			// If the first word contains metadata, then the others should also.
			// Even if they don't, it's better to make a link out of a few more words than fewer.
			if (metadata != null)
			{
				var metadata_name = metadata as string;
				if (metadata_name == null)
					return href_attribute;

				var word_metadata = Book.Metadata[metadata_name];
				if (word_metadata.Href == null)
					return href_attribute;

				// A link will be processed only if it is internal
				// In that case, it will be relinked to appropriate chapter
				// If the function fails for some reason, then no link will be added
				var attribute_link = string.Empty;
				if (!GetProcessedLink(word_metadata, ref attribute_link))
					return href_attribute;

				href_attribute.LocalName = "href";
				href_attribute.Value = attribute_link;
			}

			return href_attribute;
		}

		private string GetTextElementType(BlockContent block)
		{
			if (block.Size == null)
				return "p";

			if ((block.Size as string) != "headline")
				return "p";

			var header_level = 2;
			if (block.SizeClass != null)
			{
				const int MAX_HEADLINE_SIZE = 4;
				header_level = MAX_HEADLINE_SIZE - block.SizeClass.Value + 1;
				header_level = Math.Min(Math.Max(1, header_level), MAX_HEADLINE_SIZE);
				header_level += 1;
			}

			return "h" + header_level;
		}

		private void HandleText(BlockContent block)
		{
			// Don't do anything when the word count is 0.
			// Should not ever happen with text-type elements.
			if (block.WordCount == 0)
				return;

			var text = new List<string>();
			foreach (var word in block.Words)
				text.Add(HandleWordContent(word));

			var style_attribute = GetStyleAttribute(block);
			var class_attribute = GetClassAttribute(block.Words[0].Style);
			var href_attribute = GetHrefAttribute(block.Words[0].Metadata);

			var text_string = string.Join(" ", text);

			// If we found an URL in the metadata, then we will create a link.
			// If not, and there are size attributes, then we will create a headline
			// and, finally, if there are no size attributes, then we will create a paragraph.
			if (href_attribute.LocalName != null)
			{
				CurrentPage.Html.AddLink(
					text_string,
					href_attribute,
					class_attribute,
					style_attribute
				);
			}
			else
			{
				CurrentPage.Html.AddText(
					GetTextElementType(block),
					text_string,
					class_attribute,
					style_attribute
				);
			}
		}

		private void HandleImage(BlockContent block)
		{
			// Obtain image path and filename
			var image_path = Book.Directory +
				Path.DirectorySeparatorChar +
				CurrentPage.Chapter.FilePath +
				Path.DirectorySeparatorChar +
				block.Src;

			var image_name = Path.GetFileName(image_path);
			var image_content = File.ReadAllBytes(image_path);

			// Shouldn't happen
			if (IsCorruptImage(image_content))
				Helper.Warning("You should redownload the book. Found corrupt image: " + image_name);

			// Only add this image to our image list if it doesn't already exist
			// So we can later add it to the EPUB file
			if (!Book.Images.ContainsKey(image_name))
			{
				Helper.Debug("Adding image: " + image_name);

				Book.Images.Add(image_name, new HtmlImage
				{
					AbsoluteFilePath = image_path,
					Content = image_content,
					ID = "img" + Book.Images.Count.ToString("D4")
				});
			}

			// Add image element to HTML
			CurrentPage.Html.AddImage(
				image_name,
				block.Alt ?? image_name,
				block.Center ?? false
			);
		}

		private void HandleRow(BlockContent block)
		{
			if (!CurrentPage.InTable)
			{
				// Don't estimate the table column width. Just average it.
				// Leave it to the person post-processing the book.
				CurrentPage.Html.StartTable(block.Cells.Length);
				CurrentPage.InTable = true;
			}

			var columns = new List<HtmlTableColumn>();

			// NOTE: Only text is supported inside tables
			// Meaning, no images, links, etc. are allowed.
			foreach (var cell in block.Cells)
			{
				var text = new List<string>();

				// Similar to the BlockContent type
				foreach (var node in cell.Nodes)
				{
					if (node.WordCount == 0)
						continue;

					foreach (var word in node.Words)
						text.Add(HandleWordContent(word));
				}

				// Can occurr from time to time
				if (cell.Style != null)
				{
					if ((cell.Style as string).Contains("-scribd") && !CurrentPage.WarnedUnsupportedStyle)
					{
						Helper.Warning("Double-check table style attribute for an unsupported -scribd* property.");
						CurrentPage.WarnedUnsupportedStyle = true;
					}
				}

				columns.Add(new HtmlTableColumn
				{
					Text = string.Join(" ", text),
					Style = cell.Style
				});
			}

			CurrentPage.Html.AddTableRow(columns);
		}

		private void HandleBlocks(BlockContent block)
		{
			// Upon the finding of the first non-table element
			// stop populating the table with rows and end it.
			if (CurrentPage.InTable && block.Type != "row")
			{
				CurrentPage.Html.EndTable();
				CurrentPage.InTable = false;
			}

			switch (block.Type)
			{
				case "text":
					// Don't log when a text block is found
					HandleText(block);
					break;
				case "image":
					// Don't log when an image block is found
					HandleImage(block);
					break;
				case "raw":
					Helper.Warning("Ignoring \"raw\" block");
					break;
				case "spacer":
					CurrentPage.Html.AddSpacer((block.Size as double?) ?? 1.0);
					break;
				case "page_break":
					CurrentPage.Html.AddPageBreak();
					break;
				case "border":
					CurrentPage.Html.AddBorder((block.Width as double?) ?? 1.0, block.Style);
					break;
				case "row":
					HandleRow(block);
					break;
				case "hr":
					CurrentPage.Html.AddHorizontalRule();
					break;
				default:
					// Shouldn't ever happen
					Helper.Warning("Found an unknown block type: " + block.Type);
					break;
			}
		}

		public void Convert()
		{
			if (ShouldGenerateCoverPage)
			{
				if (!AcquireCoverPage())
					throw new Exception("An error occurred while aquiring the cover page!");
			}

			for (int i = 0; i < Book.Chapters.Length; ++i)
			{
				using var string_writer = new UTF8StringWriter();
				using var writer = XmlWriter.Create(string_writer, Helper.XmlWriterSettings);

				// Reset current page info
				CurrentPage = new HtmlPageInfo
				{
					Chapter = Book.Chapters[i],
					Html = new HtmlGenerator(writer, Book.Chapters[i].Title),
					InTable = false,
					WarnedUnsupportedStyle = false
				};

				// Iterate over blocks for this chapter
				foreach (var block in CurrentPage.Chapter.Content.Blocks)
					HandleBlocks(block);

				// Make sure to close the table (just in case)
				if (CurrentPage.InTable)
					CurrentPage.Html.EndTable();

				// Close the stream, so we can finally obtain the XML result
				writer.Close();

				// Copy generated HTML, so we can later save it to EPUB
				Book.Chapters[i].HTML = string_writer.ToString();
			}

			Helper.Debug("Finished converting MPUB blocks to HTML");
		}
	}
}