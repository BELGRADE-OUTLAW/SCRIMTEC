using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ScribdMpubToEpubConverter
{
	public static class Logger
	{
		public static readonly string[] Levels = { "Warning", "Information", "Debug" };

		public static readonly int Warning = 0;
		public static readonly int Information = 1;
		public static readonly int Debug = 2;

		// Set default level
		public static int CurrentLevel = Information;
	}

	public static class Helper
	{
		public static readonly XmlWriterSettings XmlWriterSettings = new XmlWriterSettings
		{
			Indent = true,
			IndentChars = "\t",
			CheckCharacters = true,
			Encoding = new UTF8Encoding(false)
		};

		public static BindingList<string> DebugLog = new BindingList<string>();
		public static string DebugLogPath { get; set; }
		public static int WarningCount { get; set; }

		// Little hack to keep track of warnings between conversions
		// and avoid unrequired branching
		public static void Warning(string message) => Log(Logger.Warning + (++WarningCount - WarningCount), message);
		public static void Information(string message) => Log(Logger.Information, message);
		public static void Debug(string message) => Log(Logger.Debug, message);

		private static void Log(int level, string message)
		{
			// Don't log anything
			if (level > Logger.CurrentLevel)
				return;

			var stack = new StackTrace();

			// Get function class and name from 2 stack frames up
			var previous_method = stack.GetFrame(2).GetMethod();
			var function_name = previous_method.DeclaringType.Name + "::" + previous_method.Name;

			var log_message = "[" + Logger.Levels[level] + "]" +
				"[" + function_name + "] " + message;

			// Don't log debug messages to application
			if (level != Logger.Debug)
				DebugLog.Add(log_message);

			// Log with timestamp to file
			File.AppendAllText(
				DebugLogPath,
				"[" + System.DateTime.Now.ToString() + "]" +
				log_message + "\r\n"
			);
		}

		public static void ShowWarning(string message, string title = "Warning")
		{
			MessageBox.Show(
				message,
				title,
				MessageBoxButtons.OK,
				MessageBoxIcon.Warning
			);
		}

		public static string CreateDirectory(string directory_name)
		{
			var output_directory_path =
				Directory.GetCurrentDirectory() +
				Path.DirectorySeparatorChar +
				directory_name;

			Directory.CreateDirectory(output_directory_path);
			return output_directory_path + Path.DirectorySeparatorChar;
		}
	}
}
