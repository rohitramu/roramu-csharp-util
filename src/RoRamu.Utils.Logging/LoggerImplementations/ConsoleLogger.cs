namespace RoRamu.Utils.Logging
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using RoRamu.Utils;

    /// <summary>
    /// A console logger.
    /// </summary>
    public class ConsoleLogger : Logger
    {
        /// <summary>
        /// Whether or not to log extra details.
        /// </summary>
        /// <value></value>
        public static bool LogExtraInfo { get; set; } = false;

        private static readonly int LongestLogLevelName = Enum.GetNames(typeof(LogLevel)).Select(name => name.Length).Max();

        /// <inheritdoc/>
        protected override void HandleLog<T>(
            LogLevel logLevel,
            string message,
            T extraInfo,
            string callerName,
            string sourceFilePath,
            int sourceLineNumber)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string indentToken = " ";
            int indentSize = timestamp.Length + LongestLogLevelName + 4;

            string logMessage = $"{timestamp} [{logLevel}]".PadRight(indentSize - 1);
            if (message != null)
            {
                logMessage += $" {message}";
            }

            if (ConsoleLogger.LogExtraInfo)
            {
                IList<IEnumerable<string>> sections = new List<IEnumerable<string>>
                {
                    // Create the lines in the header text
                    new string[]
                    {
                        $"Call: {callerName}()",
                        $"File: {sourceFilePath}",
                        $"Line: {sourceLineNumber}",
                    }
                };

                // Read the lines in the main text
                if (extraInfo != null)
                {
                    // Get the name of the extraInfo object's type
                    sections.Add(extraInfo.GetType().FullName.SingleObjectAsEnumerable());

                    IList<string> mainText = new List<string>();
                    using (StringReader reader = new StringReader(extraInfo.ToString()))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            mainText.Add(line);
                        }
                    }
                    sections.Add(mainText);
                }

                // Create a separator based on the longest line in the text
                int separatorLength = sections.Select(lines => lines.Select(line => line.Length).Max()).Max();

                // Create the separator
                string separator = $"+{new string('-', separatorLength)}+";

                // Create a string builder to construct the result
                StringBuilder stringBuilder = new StringBuilder();

                // Build the string from the sections
                stringBuilder.AppendLine(separator);
                foreach (IEnumerable<string> lines in sections)
                {
                    // Append all the lines in this section
                    foreach (string line in lines)
                    {
                        stringBuilder.AppendLine($"|{line.PadRight(separatorLength)}|");
                    }

                    // Add separator
                    stringBuilder.AppendLine(separator);
                }

                // Compile the log text
                logMessage += $"\n{stringBuilder.ToString().Indent(indentSize, indentToken)}\n";
            }

            Console.WriteLine(logMessage);
        }
    }
}
