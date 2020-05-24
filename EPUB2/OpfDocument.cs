using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ScribdMpubToEpubConverter.EPUB2
{
	struct ManifestItem
	{
		public string ID { get; set; }
		// NOTE: Relative to the OEBPS/ folder
		public string FilePath { get; set; }
	}

	struct GuideItem
	{
		// NOTE: Relative to the OEBPS/ folder
		public string Type { get; set; }
		public string Title { get; set; }
		public string FilePath { get; set; }
	}

	class OpfDocument : IEpubDocument
	{
		private static readonly Dictionary<string, string> FileTypes = new Dictionary<string, string>
		{
			{ "html", "application/xhtml+xml" },
			{ "xhtml", "application/xhtml+xml" },
			{ "ncx", "application/x-dtbncx+xml" },
			{ "css", "text/css" },
			{ "jpg", "image/jpeg" },
			{ "jpeg", "image/jpeg" },
			{ "png", "image/png" },
			{ "gif", "image/gif" },
			{ "svg", "image/svg+xml" }
		};

		public List<GuideItem> GuideItems = new List<GuideItem>();
		public List<ManifestItem> ManifestItems = new List<ManifestItem>();

		public string Title { get; set; }

		public override DocumentType Type => DocumentType.OPF;

		private string GetFileMimeType(string path)
		{
			foreach (var file_type in FileTypes)
			{
				if (path.EndsWith(file_type.Key))
					return file_type.Value;
			}

			// Shouldn't ever happen
			throw new Exception("GetFileMimeType() encountered an invalid file type\n" + path);
		}

		private void WriteDocumentTitle(XmlWriter writer, string title)
		{
			/*
			 * <dc:title id="title">@title</@dc:title>
			 */
			writer.WriteStartElement("dc", "title", null);
			writer.WriteAttributeString("id", "title");
			writer.WriteString(title);
			writer.WriteEndElement();
		}

		// TODO: Write additional metadata?
		private void WriteMetadata(XmlWriter writer)
		{
			/*
			 * <metadata
			 *		xmlns:opf="@Constants.XmlOpfNamespaceURL"
			 *		xmlns:dc="@Constants.XmlDublinCoreURL"
			 *		xmlns:dcterms="@Constants.XmlDublinCoreTermsURL">
			 */
			writer.WriteStartElement("metadata");
			writer.WriteAttributeString("xmlns", "opf", null, Constants.XmlOpfNamespaceURL);
			writer.WriteAttributeString("xmlns", "dc", null, Constants.XmlDublinCoreURL);
			writer.WriteAttributeString("xmlns", "dcterms", null, Constants.XmlDublinCoreTermsURL);

			WriteDocumentTitle(writer, Title);

			/*
			 * <dc:language>en-US</dc:language>
			 */
			writer.WriteElementString("dc", "language", null, "en-US");

			/*
			 * <dc:date opf:event="modification">@CurrentTimeInISO8601</dc:date>
			 */
			writer.WriteStartElement("dc", "date", null);
			writer.WriteAttributeString("opf", "event", null, "modification");
			writer.WriteString(DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
			writer.WriteEndElement();

			/*
			 * <dc:contributor opf:role="bkp">ScribdMpubToEpubConverter</dc:contributor>
			 */
			writer.WriteStartElement("dc", "contributor", null);
			writer.WriteAttributeString("opf", "role", null, "bkp");
			writer.WriteString("ScribdMpubToEpubConverter");
			writer.WriteEndElement();

			/*
			 * <dc:source>Scribd</dc:source>
			 */
			writer.WriteElementString("dc", "source", null, "Scribd");

			/*
			 * <dc:identifier id="uid" opf:scheme="ISBN">0000000000000</dc:identifier>
			 */
			writer.WriteStartElement("dc", "identifier", null);
			writer.WriteAttributeString("id", "uid");
			writer.WriteAttributeString("opf", "scheme", null, "ISBN");
			writer.WriteString("0000000000000");
			writer.WriteEndElement();

			/*
			 * </metadata>
			 */
			writer.WriteEndElement();
		}

		private void WriteManifest(XmlWriter writer)
		{
			/*
			 * <manifest>
			 */
			writer.WriteStartElement("manifest");

			foreach (var item in ManifestItems)
			{
				/*
				 * <item id="@id" href="@path" media-type"@GetFileMediaType(path)" />
				 */
				writer.WriteStartElement("item");
				writer.WriteAttributeString("id", item.ID);
				writer.WriteAttributeString("href", item.FilePath);
				writer.WriteAttributeString("media-type", GetFileMimeType(item.FilePath));
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}

		private void WriteSpine(XmlWriter writer)
		{
			/*
			 * <spine>
			 */
			writer.WriteStartElement("spine");
			writer.WriteAttributeString("toc", "ncx");

			foreach (var item in ManifestItems)
			{
				/*
				 * The <spine> element can consist only of HTML files.
				 * 
				 * NOTE:
				 * Each itemref in spine must not reference media types other than
				 * OPS Content Documents (or documents whose fallback chain includes an OPS Content Document).
				 * An OPS Content Document must be of one of the following media types:
				 *  - application/xhtml+xml,
				 *  - application/x-dtbook+xml,
				 *  - the deprecated text/x-oeb1-document,
				 *  - and Out-Of-Line XML Island (with required fallback.)
				 * When a document with a media type not from this list (or a document whose fallback chain doesn't include a document with a media type from this list) is referenced in spine, Reading Systems must not include it as part of the spine.
				 */
				var mimetype = GetFileMimeType(item.FilePath);
				if (mimetype != "application/xhtml+xml")
					continue;

				/*
				 * <itemref idref="@item.ID" />
				 */
				writer.WriteStartElement("itemref");
				writer.WriteAttributeString("idref", item.ID);
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}

		private void WriteGuide(XmlWriter writer)
		{
			// Don't do anything if there are no guide items
			if (GuideItems.Count == 0)
				return;

			/*
			 * <guide>
			 */
			writer.WriteStartElement("guide");

			foreach (var item in GuideItems)
			{
				/*
				 * <reference href="@item.FilePath" type="@item.Type" title="@item.Title"/>
				 */
				writer.WriteStartElement("reference");
				writer.WriteAttributeString("type", item.Type);
				writer.WriteAttributeString("title", item.Title);
				writer.WriteAttributeString("href", item.FilePath);
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}

		public void AddManifestItem(string id, string file_path)
		{
			ManifestItems.Add(new ManifestItem
			{
				ID = id,
				FilePath = file_path
			});
		}

		public override void Write(Stream stream)
		{
			using var writer = XmlWriter.Create(stream, Helper.XmlWriterSettings);

			/*
			* <?xml version="1.0" encoding="utf-8"?>
			*/
			writer.WriteStartDocument();

			/* <package
			 *		xmlns="@Constants.XmlOpfNamespaceURL"
			 *		xmlns:dc="@Constants.XmlDublinCoreURL"
			 *		unique-identifier="uid"
			 *		version="2.0">
			 */
			writer.WriteStartElement("package", Constants.XmlOpfNamespaceURL);
			writer.WriteAttributeString("unique-identifier", "uid");
			writer.WriteAttributeString("version", "2.0");

			WriteMetadata(writer);
			WriteManifest(writer);
			WriteSpine(writer);
			WriteGuide(writer);

			/*
			 * </package>
			 */
			writer.WriteEndElement();
			writer.WriteEndDocument();
		}
	}
}
