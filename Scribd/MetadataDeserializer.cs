using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace ScribdMpubToEpubConverter.Scribd
{
	// We can use the dynamic keyword, but I find it a matter of preference.
#nullable enable
	struct MetadataContent
	{
		[JsonProperty("color")]
		public int[]? Color { get; set; }

		[JsonProperty("external_link")]
		public bool? ExternalLink { get; set; }

		[JsonProperty("href")]
		public string? Href { get; set; }

		[JsonProperty("superscript")]
		public int? Superscript { get; set; }

		[JsonProperty("underline")]
		public bool? Underline { get; set; }

		[JsonProperty("overline")]
		public bool? Overline { get; set; }
	}
#nullable restore

	class MetadataDeserializer
	{
		public static Dictionary<string, MetadataContent> Deserialize(string content)
		{
			var json = JObject.Parse(content);

			var results = new Dictionary<string, MetadataContent>();
			foreach (var metadata_entry in json["metadata"].Children())
			{
				var entry_name = metadata_entry.Value<string>();

				results.Add(
					entry_name,
					JsonConvert.DeserializeObject<MetadataContent>(
						json[entry_name].ToString()
					)
				);
			}

			return results;
		}
	}
}
