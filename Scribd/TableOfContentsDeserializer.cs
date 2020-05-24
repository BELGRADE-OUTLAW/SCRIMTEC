using Newtonsoft.Json;

namespace ScribdMpubToEpubConverter.Scribd
{
	struct AdditionalChapterInformation
	{
		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("filepath")]
		public string FilePath { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("block_start")]
		public int BlockStart { get; set; }

		[JsonProperty("block_end")]
		public int BlockEnd { get; set; }
	}

	class TableOfContentsDeserializer
	{
		public static AdditionalChapterInformation[] Deserialize(string content)
		{
			return JsonConvert.DeserializeObject<AdditionalChapterInformation[]>(content);
		}
	}
}
