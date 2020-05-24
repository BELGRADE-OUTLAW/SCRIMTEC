using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace ScribdMpubToEpubConverter
{
	public sealed class UTF8StringWriter : StringWriter
	{
		public override Encoding Encoding
		{
			get { return Encoding.UTF8; }
		}
	}

	struct HtmlTableColumn
	{
		public string Text { get; set; }
		public string Style { get; set; }
	}

	struct HtmlAttribute
	{
		public string LocalName { get; set; }
		public string Value { get; set; }
	}

	class HtmlGenerator
	{
		private XmlWriter Writer { get; set; }

		public HtmlGenerator(XmlWriter writer, string title)
		{
			Writer = writer;

			/*
			 * <?xml version="1.0" encoding="utf-8" standalone="no"?>
			 */
			Writer.WriteStartDocument(false);

			/*
			 * <!DOCTYPE html
			 *		PUBLIC
			 *		"-//W3C//DTD XHTML 1.1//EN"
			 *		"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
			 */
			Writer.WriteDocType("html", "-//W3C//DTD XHTML 1.1//EN", "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", null);

			/*
			 * <html>
			 */
			Writer.WriteStartElement("html", "http://www.w3.org/1999/xhtml");

			/*
			 * <head>
			 */
			Writer.WriteStartElement("head");

			/*
			 * <link href="../Styles/style.css" rel="stylesheet" type="text/css"/>
			 */
			var styles_relative_path = EPUB2.EpubDirectories.OEBPS_Styles.Replace(
				EPUB2.EpubDirectories.OEBPS,
				"../"
			);
			Writer.WriteStartElement("link");
			Writer.WriteAttributeString("href", styles_relative_path + "style.css");
			Writer.WriteAttributeString("rel", "stylesheet");
			Writer.WriteAttributeString("type", "text/css");
			Writer.WriteEndElement();

			/*
			 * <title>@title</title>
			 */
			Writer.WriteElementString("title", title);
			Writer.WriteEndElement();

			/*
			 * <body>
			 */
			Writer.WriteStartElement("body");
		}

		public void AddImage(string filename, string alt, bool should_center)
		{
			/*
			 * <div @should_center>
			 *		<img src="../Images/@filename" alt="@alt" height="100%" />
			 * </div>
			 */
			Writer.WriteStartElement("div");

			// Center image if needed
			if (should_center)
				Writer.WriteAttributeString("style", "text-align: center;");

			Writer.WriteStartElement("img");
			Writer.WriteAttributeString("src", "../Images/" + filename);
			Writer.WriteAttributeString("alt", alt);
			Writer.WriteAttributeString("height", "100%");
			Writer.WriteEndElement();
			Writer.WriteEndElement();
		}

		public void AddText(string element, string content, params HtmlAttribute[] attributes)
		{
			/*
			 * <@element @attributes...>@content</@element>
			 */
			Writer.WriteStartElement(element);
			if (attributes != null)
			{
				foreach (var attribute in attributes)
				{
					// HACK! Don't add attributes with empty names.
					if (attribute.LocalName == null)
						continue;

					Writer.WriteAttributeString(attribute.LocalName, attribute.Value);
				}
			}
			Writer.WriteString(content);
			Writer.WriteEndElement();
		}

		// NOTE: Only the attribute with the LocalName "href" will
		// be added to the <a> element, all other will be added to the <p> element.
		public void AddLink(string content, params HtmlAttribute[] attributes)
		{
			/*
			 * <p @attributes...>
			 *		<a @attributes["href"]>@content</a>
			 * </p>
			 */
			Writer.WriteStartElement("p");
			if (attributes != null)
			{
				foreach (var attribute in attributes)
				{
					// HACK! Don't add attributes with empty names.
					if (attribute.LocalName == null)
						continue;

					// Don't add href attribute to <p>
					if (attribute.LocalName == "href")
						continue;

					Writer.WriteAttributeString(attribute.LocalName, attribute.Value);
				}
			}

			Writer.WriteStartElement("a");
			if (attributes != null)
			{
				foreach (var attribute in attributes)
				{
					// HACK! Don't add attributes with empty names.
					if (attribute.LocalName == null)
						continue;

					// Add only href attribute to <a>
					if (attribute.LocalName != "href")
						continue;

					Writer.WriteAttributeString(attribute.LocalName, attribute.Value);
				}
			}
			Writer.WriteString(content);
			Writer.WriteEndElement();
			Writer.WriteEndElement();
		}

		public void AddPageBreak()
		{
			/*
			 * <div style="page-break-before: always;" />
			 */
			Writer.WriteStartElement("div");
			Writer.WriteAttributeString("style", "page-break-before: always;");
			Writer.WriteEndElement();
		}

		public void AddBorder(double width, string style)
		{
			/*
			 * <div style="@style_attribute" />
			 */
			var style_attribute = "border-style: " + style +
				"; border-color: rgb(0, 0, 0); border-top-width: 1px; width: " +
				width.ToString() + ";";

			Writer.WriteStartElement("div");
			Writer.WriteAttributeString("style", style_attribute);
			Writer.WriteEndElement();
		}

		public void AddHorizontalRule()
		{
			/*
			 * <hr/>
			 */
			Writer.WriteStartElement("hr");
			Writer.WriteEndElement();
		}

		public void AddSpacer(double size)
		{
			/*
			 * <br @size;"/>
			 */
			Writer.WriteStartElement("br");
			if (size != 1)
				Writer.WriteAttributeString("style", "line-height: " + size);
			Writer.WriteEndElement();
		}

		public void StartTable(int column_count)
		{
			/*
			 * <table style="@TableStyle">
			 *		<colgroup>
			 *			...
			 *			<col width="@average_colum_width@ />
			 *			...
			 *		</colgroup>
			 */
			const string TableStyle = "table-layout: fixed; white-space: nowrap; border-collapse: collapse; border-spacing: 0px; margin: 5% auto;";

			// Calculate average width in procentage
			var average_column_width = 100.0 / column_count;

			Writer.WriteStartElement("table");
			Writer.WriteAttributeString("style", TableStyle);
			Writer.WriteStartElement("colgroup");
			for (int i = 0; i < column_count; ++i)
			{
				Writer.WriteStartElement("col");
				Writer.WriteAttributeString("width", average_column_width.ToString("0.##") + "%");
				Writer.WriteEndElement();
			}
			Writer.WriteEndElement();
		}

		public void AddTableRow(List<HtmlTableColumn> columns)
		{
			/*
			 * <tr>
			 *		...
			 *		<td style="@column.Style">@column.Text</td>
			 *		...
			 * </tr>
			 */
			Writer.WriteStartElement("tr");

			foreach (var column in columns)
			{
				Writer.WriteStartElement("td");

				// Set style attribute, if there is a style present
				if (column.Style != null)
					Writer.WriteAttributeString("style", column.Style);

				Writer.WriteString(column.Text);
				Writer.WriteEndElement();
			}

			Writer.WriteEndElement();
		}

		public void EndTable()
		{
			/*
			 * </table>
			 */
			Writer.WriteEndElement();
			AddPageBreak();
		}
	}
}