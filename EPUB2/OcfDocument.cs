using System.IO;
using System.Xml;

namespace ScribdMpubToEpubConverter.EPUB2
{
	class OcfDocument : IEpubDocument
	{
		public override DocumentType Type => DocumentType.OCF;

		public override void Write(Stream stream)
		{
			using var writer = XmlWriter.Create(stream, Helper.XmlWriterSettings);

			/*
			* <?xml version="1.0" encoding="utf-8" standalone="yes"?>
			*/
			writer.WriteStartDocument(true);

			/*
			 * <container xmlns="@Constants.XmlContainerNamespaceURL" version="1.0">
			 */
			writer.WriteStartElement("container", Constants.XmlContainerNamespaceURL);
			writer.WriteAttributeString("version", "1.0");

			/*
			 * <rootfiles>
			 */
			writer.WriteStartElement("rootfiles");

			/*
			 * <rootfile full-path="@opf_document_path" media-type="application/oebps-package+xml" />
			 */
			var opf_document_path = DocumentTypeExtensions.GetRelativePath(DocumentType.OPF);
			writer.WriteStartElement("rootfile");
			writer.WriteAttributeString("full-path", opf_document_path);
			writer.WriteAttributeString("media-type", "application/oebps-package+xml");
			writer.WriteEndElement();

			/*
			 * </rootfiles>
			 */
			writer.WriteEndElement();

			/*
			 * </container>
			 */
			writer.WriteEndElement();
			writer.WriteEndDocument();
		}
	}
}
