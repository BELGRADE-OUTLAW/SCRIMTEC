using System.Collections.Generic;
using System.IO;

namespace ScribdMpubToEpubConverter.EPUB2
{
	// NOTE: These are relative paths
	public static class EpubDirectories
	{
		public static readonly string METAINF = "META-INF/";
		public static readonly string OEBPS = "OEBPS/";
		public static readonly string OEBPS_Text = OEBPS + "Text/";
		public static readonly string OEBPS_Styles = OEBPS + "Styles/";
		public static readonly string OEBPS_Images = OEBPS + "Images/";

		public static readonly string[] Directories = {
			METAINF,
			OEBPS,
			OEBPS_Text,
			OEBPS_Images,
			OEBPS_Styles
		};
	}

	public enum DocumentType
	{
		OCF,
		OPF,
		NCX
	};

	public struct DocumentInfo
	{
		public string Name { get; set; }
		public string FolderName { get; set; }
	}

	public static class DocumentTypeExtensions
	{
		public static readonly Dictionary<DocumentType, DocumentInfo> Documents = new Dictionary<DocumentType, DocumentInfo>
		{
			{
				DocumentType.OCF,
				new DocumentInfo{ Name = "container.xml", FolderName = EpubDirectories.METAINF }
			},
			{
				DocumentType.OPF,
				new DocumentInfo{ Name = "content.opf", FolderName = EpubDirectories.OEBPS }
			},
			{
				DocumentType.NCX,
				new DocumentInfo { Name = "toc.ncx", FolderName = EpubDirectories.OEBPS }
			}
		};

		public static string GetRelativePath(this DocumentType type)
		{
			return Documents[type].FolderName + Documents[type].Name;
		}
	}

	public abstract class IEpubDocument
	{
		public abstract DocumentType Type { get; }

		public abstract void Write(Stream stream);
	}
}
