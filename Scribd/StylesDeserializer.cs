using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace ScribdMpubToEpubConverter.Scribd
{
	class StylesDeserializer
	{
		public static Dictionary<string, string> Deserialize(string content)
		{
			var json = JObject.Parse(content);

			var results = new Dictionary<string, string>();
			foreach (var metadata_entry in json["styles"].Children())
			{
				var entry_name = metadata_entry.Value<string>();
				var entry_value = json[entry_name].Value<string>().Trim();

				// Don't add empty styles
				if (entry_value.Length == 0)
				{
					Helper.Information("Skipping empty style entry: " + entry_name);
					continue;
				}

				results.Add(entry_name, entry_value);
			}

			return results;
		}
	}
}