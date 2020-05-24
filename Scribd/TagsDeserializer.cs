using Newtonsoft.Json;
using System.Collections.Generic;

namespace ScribdMpubToEpubConverter.Scribd
{
	struct TagContent
	{
		[JsonProperty("block_idx")]
		public int BlockIndex { get; set; }
	}

	struct TagsContent
	{
		[JsonProperty("tags")]
		public Dictionary<string, TagContent> Tags { get; set; }
	}

	class TagsDeserializer
	{
		public static TagsContent Deserialize(string content)
		{
			return JsonConvert.DeserializeObject<TagsContent>(content);
		}
	}
}