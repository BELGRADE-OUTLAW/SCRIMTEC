using Newtonsoft.Json;

namespace ScribdMpubToEpubConverter.Scribd
{
#nullable enable
	/*
	 * The following properties are not deserialized:
	 * 
	 *	[JsonProperty("break_map")]
	 *	public BreakMapContent? BreakMap { get; set; }
	 * 
	 *	[JsonProperty("font")]
	 *	public string? Font { get; set; }
	 */
	struct WordContent
	{
		// break_map not included
		[JsonProperty("id")]
		public int? ID { get; set; }

		[JsonProperty("metadata")]
		public string? Metadata { get; set; }

		// Incompetency at it's finest. When you include an image in a Words[] array element.
		// The use of dynamic here is _absolutely required_.
		[JsonProperty("style")]
		public dynamic? Style { get; set; }

		[JsonProperty("text")]
		public string? Text { get; set; }

		// i.e. simple, composite, image
		[JsonProperty("type")]
		public string? Type { get; set; }

		// This element can be recursive, when the type is set to composite
		[JsonProperty("words")]
		public WordContent[]? Words { get; set; }
	}

	/*
	 * The following property is not deserialized:
	 * 
	 *	[JsonProperty("word_count")]
	 *	public int? WordCount { get; set; }
	 */
	struct CellContent
	{
		[JsonProperty("nodes")]
		public BlockContent[]? Nodes { get; set; }

		[JsonProperty("style")]
		public dynamic Style { get; set; }
	}

	/*
	 * The following properties are not deserialized:
	 * 
	 *	[JsonProperty("color")]
	 *	public string? Color { get; set; }
	 *	
	 *	[JsonProperty("orientation")]
	 *	public int Orientation { get; set; }
	 *	
	 *	[JsonProperty("rgb_color")]
	 *	public int[]? RgbColor { get; set; }
	 *	
	 *	[JsonProperty("height")]
	 *	public int? Height { get; set; }
	 *	
	 *	[JsonProperty("right_indent")]
	 *	public float? RightIndent { get; set; }
	 *	
	 *	[JsonProperty("block_indent")]
	 *	public float? BlockIndent { get; set; }
	 */
	struct BlockContent
	{
		[JsonProperty("align")]
		public string? Align { get; set; }

		[JsonProperty("alt")]
		public string? Alt { get; set; }

		// Indicates that this is a table row
		[JsonProperty("cells")]
		public CellContent[]? Cells { get; set; }

		// Only images have this property
		[JsonProperty("center")]
		public bool? Center { get; set; }

		// When type is equal to "text", then size is a string.
		// Otherwise, it's an int.
		[JsonProperty("size")]
		public dynamic? Size { get; set; }

		[JsonProperty("size_class")]
		public int? SizeClass { get; set; }

		[JsonProperty("src")]
		public string? Src { get; set; }

		// Usually the type is Dictionary<string, string>
		// Unless the type is i.e. border, then the type is just string
		[JsonProperty("style")]
		public dynamic? Style { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		// Should be int
		[JsonProperty("width")]
		public dynamic? Width { get; set; }

		[JsonProperty("word_count")]
		public int WordCount { get; set; }

		[JsonProperty("words")]
		public WordContent[]? Words { get; set; }
	}

	struct ChapterContent
	{
		[JsonProperty("blocks")]
		public BlockContent[]? Blocks { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }
	}
#nullable restore

	class ChapterContentDeserializer
	{
		// Read larger files in fragments
		public static ChapterContent Deserialize(System.IO.StreamReader reader)
		{
			using var json_reader = new JsonTextReader(reader);
			var serializer = new JsonSerializer();
			try
			{
				return serializer.Deserialize<ChapterContent>(json_reader);
			}
			catch
			{
				// They HAD to mess up something so simple!
				return new ChapterContent
				{
					Blocks = serializer.Deserialize<BlockContent[]>(json_reader),
					Title = System.IO.Path.GetRandomFileName().Replace(".", "")
				};
			}
		}
	}
}