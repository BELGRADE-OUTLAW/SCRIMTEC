using System;
using System.IO;
using System.Security.Cryptography;

namespace ScribdMpubToEpubConverter.Scribd
{
	// NOTE: AES-ECB does not require an IV.
	class Decryptor
	{
		private static string[] ProbablyEncryptedFiles =
		{
			"contents.json", ".jpg", ".jpeg", ".png", ".gif", ".svg"
		};

		public const int KEY_SIZE = 32;

		public byte[] Key { get; set; }

		// Check if file is named simply "content"
		// If it is not, then the file is probably not encoded
		// If it is, then check if the first line starts with %PDF
		// If it does not, then the file is encoded, otherwise it's not encoded
		private bool IsFileEncoded(string file_path)
		{
			if (!file_path.EndsWith("content"))
				return false;

			using var stream_reader = new StreamReader(File.OpenRead(file_path));
			return !stream_reader.ReadLine().StartsWith("%PDF");
		}

		private bool IsFileEncrypted(string file_path)
		{
			// Check if a file is encrypted based on it's name/extension
			foreach (var file in ProbablyEncryptedFiles)
			{
				if (file_path.EndsWith(file))
					return true;
			}

			return false;
		}

		public void Decrypt(string input_directory_path)
		{
			var file_list = Directory.GetFiles(input_directory_path);
			foreach (var file_path in file_list)
			{
				// Decode/Decrypt as needed
				if (IsFileEncoded(file_path))
				{
					DecodeFile(file_path);
				}
				else if (IsFileEncrypted(file_path))
				{
					DecryptFile(file_path);
				}
			}

			var subdirectory_list = Directory.GetDirectories(input_directory_path);
			foreach (var subdirectory in subdirectory_list)
				Decrypt(subdirectory);
		}

		public void GeneratePrivateKey(string input_string)
		{
			var input_length = input_string.Length;

			Key = new byte[input_length / 2];
			for (var i = 0; i < input_length; i += 2)
			{
				var var4 = i / 2;

				Key[var4] = (byte)(
					(Convert.ToInt32(input_string[i].ToString(), 16) << 4) +
					Convert.ToInt32(input_string[i + 1].ToString(), 16)
				);

				var var6 = (i == 0) ? 31 : Key[var4 - 1];
				Key[var4] = (byte)(Key[var4] ^ var6);
			}

			Helper.Debug("Generated private key for entry " + input_string);
		}

		private void DecryptFile(string input_file_path)
		{
			Helper.Debug("Decrypting file: " + input_file_path);

			try
			{
				using var aes = new RijndaelManaged
				{
					// Set appropriate key size in bits
					KeySize = KEY_SIZE * 8,

					// The correct mode is AES-ECB PKCS5,
					// but PKCS7 and PKCS5 backwards-compatible
					Mode = CipherMode.ECB,
					Padding = PaddingMode.PKCS7,

					// Set key data
					Key = Key
				};

				// Get encrypted file content
				var encrypted_content = File.ReadAllBytes(input_file_path);

				var decryptor = aes.CreateDecryptor();
				var decrypted_data = decryptor.TransformFinalBlock(
					encrypted_content, 0,
					encrypted_content.Length
				);

				File.WriteAllBytes(input_file_path, decrypted_data);
			}
			catch
			{
				// Most likely the document wasn't encrypted and the
				// AES decryptor threw an exception. Just handle it quietly.
				Helper.Debug("Skipping decryption, as the file is probably already decrypted");
			}
		}

		private void DecodeFile(string input_file_path)
		{
			Helper.Debug("Decoding file: " + input_file_path);

			// Get encoded file content
			var encoded_content = File.ReadAllBytes(input_file_path);
			for (int i = 0; i < encoded_content.Length; ++i)
				encoded_content[i] ^= Key[i % Key.Length];

			File.WriteAllBytes(input_file_path + ".pdf", encoded_content);
		}
	}
}
