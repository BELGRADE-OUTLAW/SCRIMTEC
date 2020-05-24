using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ScribdMpubToEpubConverter
{
	public static class DecryptionKeyListParser
	{
		public static void Parse(string file_path, ref Dictionary<string, string> DecryptionKeys)
		{
			using var reader = XmlReader.Create(File.OpenRead(file_path));
			while (reader.Read())
			{
				if (reader.NodeType != XmlNodeType.Element)
					continue;

				if (reader.Name != "string")
					continue;

				// Make sure to filter out the IS_MIGRATED option
				var name = reader.GetAttribute("name");
				if (!name.StartsWith("KEY_"))
				{
					Helper.Information("Skipping XML entry, as it is not a key: " + name);
					continue;
				}

				// Try to read the value
				if (!reader.Read())
					continue;

				if (reader.NodeType != XmlNodeType.Text)
					continue;

				// Sanity check, as the key length should be 32 bytes.
				var value = reader.Value.Trim();
				if (value.Length != 32)
				{
					Helper.Warning("Skipping decryption key with incorrect value length: " + name);
					continue;
				}

				DecryptionKeys.Add(name.Remove(0, "KEY_".Length), value);
			}

			Helper.Debug("Decryption key entries: " + DecryptionKeys.Count);
		}
	}
}
