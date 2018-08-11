namespace RoRamu.Utils.Logging
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using RoRamu.Utils;

    public class ConsoleLogger : Logger
    {
        private static readonly int LongestLogLevelName = Enum.GetNames(typeof(LogLevel)).Select(name => name.Length).Max();

        protected override void HandleLog<T>(
            LogLevel logLevel,
            string message,
            T extraInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string indentToken = " ";
            int indentSize = timestamp.Length + LongestLogLevelName + 4;

            string logMessage = $"{timestamp} [{logLevel.ToString()}]".PadRight(indentSize - 1);
            if (message != null)
            {
                logMessage += $" {message}";
            }

            if (extraInfo != null)
            {
                // Create the lines in the header text
                string[] headerText = new string[]
                {
                    $"Call:   {memberName}()",
                    $"File:   {sourceFilePath}",
                    $"Line:   {sourceLineNumber}",
                };

                // Read the lines in the main text
                IList<string> mainText = new List<string>();
                using (StringReader reader = new StringReader(extraInfo.ToString()))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        mainText.Add(line);
                    }
                }

                // Create a separator based on the longest line in the text
                int separatorLength = Math.Max(
                    headerText.Select(line => line.Length).Max(), // header text width
                    mainText.Select(line => line.Length).Max()); // main text width

                string separator = $"+{new string('-', separatorLength)}+";

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(separator);
                foreach (string line in headerText)
                {
                    // Header text
                    stringBuilder.AppendLine($"|{line.PadRight(separatorLength)}|");
                }
                stringBuilder.AppendLine(separator);
                foreach (string line in mainText)
                {
                    // Main text
                    stringBuilder.AppendLine($"|{line.PadRight(separatorLength)}|");
                }
                stringBuilder.AppendLine(separator);

                // Compile the log text
                logMessage += $"\n{stringBuilder.ToString().Indent(indentSize, indentToken)}\n";
            }

            Console.WriteLine(logMessage);
        }
    }
}
