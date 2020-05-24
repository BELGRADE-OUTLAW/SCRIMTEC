using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ScribdMpubToEpubConverter.EPUB2
{
	struct NavigationItem
	{
		public string ID { get; set; }
		public string Label { get; set; }

		// NOTE: This is a relative file path
		public string FilePath { get; set; }
	}

	class NcxDocument : IEpubDocument
	{
		public string Title { get; set; }
		public List<NavigationItem> NavigationItems = new List<NavigationItem>();

		public override DocumentType Type => DocumentType.NCX;

		private void WriteHead(XmlWriter writer)
		{
			void write_meta(string name, string content)
			{
				/**
				 * <meta name="@name" content="@content" />
				 */
				writer.WriteStartElement("meta");
				writer.WriteAttributeString("name", name);
				writer.WriteAttributeString("content", content);
				writer.WriteEndElement();
			}

			/**
			 * <head>
			 */
			writer.WriteStartElement("head");

			//
			//foreach (var metadata in MetadataItems)
			//	write_meta(metadata.Name, metadata.Content);

			write_meta("dtb:uid", "0000000000000");
			write_meta("dtb:depth", "1");
			write_meta("dtb:totalPageCount", "0");
			write_meta("dtb:maxPageNumber", "0");

			/*
			 * </head>
			 */
			writer.WriteEndElement();
		}

		private void WriteDocumentText(XmlWriter writer, string element_name, string content)
		{
			/*
			 * <@element_name>
			 *		<text>@content</text>
			 * </@element_name>
			 */
			writer.WriteStartElement(element_name);
			writer.WriteElementString("text", content);
			writer.WriteEndElement();
		}

		private void WriteNavigationMap(XmlWriter writer)
		{
			void write_nav_item(NavigationItem item, int id)
			{
				/*
				 * <navPoint id="navpoint-@id" playOrder="@id">
				 */
				writer.WriteStartElement("navPoint");
				writer.WriteAttributeString("id", "navpoint-" + item.ID);
				writer.WriteAttributeString("playOrder", id.ToString());

				/*
				 * <navLabel>
				 *		<text>@label</text>
				 * </navLabel>
				 */
				writer.WriteStartElement("navLabel");
				writer.WriteElementString("text", item.Label);
				writer.WriteEndElement();

				/*
				 * <content src="@path" />
				 */
				writer.WriteStartElement("content");
				writer.WriteAttributeString("src", item.FilePath);
				writer.WriteEndElement();

				/*
				 * </navPoint>
				 */
				writer.WriteEndElement();
			}

			/*
			 * <navMap>
			 */
			writer.WriteStartElement("navMap");

			for (int i = 0; i < NavigationItems.Count; ++i)
				write_nav_item(NavigationItems[i], i);

			/*
			 * </navMap>
			 */
			writer.WriteEndElement();
		}

		public void AddNavigationItem(string id, string label, string file_path)
		{
			NavigationItems.Add(new NavigationItem
			{
				ID = id,
				Label = label,
				FilePath = file_path
			});
		}

		// NOTE: NCX DOCTYPE intentionally omitted.
		// It is not necessary as per the EPUB2.0.1 specification.
		public override void Write(Stream stream)
		{
			using var writer = XmlWriter.Create(stream, Helper.XmlWriterSettings);

			/*
			* <?xml version="1.0" encoding="utf-8"?>
			*/
			writer.WriteStartDocument();

			/*
			 * <!DOCTYPE ncx
			 *		PUBLIC
			 *		"-//NISO//DTD ncx 2005-1//EN"
			 *		"http://www.daisy.org/z3986/2005/ncx-2005-1.dtd">
			 */
			writer.WriteDocType("ncx", "-//NISO//DTD ncx 2005-1//EN", "http://www.daisy.org/z3986/2005/ncx-2005-1.dtd", null);

			/*
			 * <ncx xmlns="@Constants.XmlNcxNamespaceURL" version="2005-1">
			 */
			writer.WriteStartElement("ncx", Constants.XmlNcxNamespaceURL);
			writer.WriteAttributeString("version", "2005-1");

			WriteHead(writer);
			WriteDocumentText(writer, "docTitle", Title);
			WriteNavigationMap(writer);

			/*
			 * </ncx>
			 */
			writer.WriteEndElement();
			writer.WriteEndDocument();
		}
	}
}
