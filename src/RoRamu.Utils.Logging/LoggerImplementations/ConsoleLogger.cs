namespace RoRamu.Utils.Logging
{
    using System;
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
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("========");
                stringBuilder.AppendLine($"== Call: {memberName}()");
                stringBuilder.AppendLine($"== File:   {sourceFilePath}");
                stringBuilder.AppendLine($"== Line:   {sourceLineNumber}");
                stringBuilder.AppendLine("========");
                stringBuilder.AppendLine(extraInfo.ToString());

                logMessage += $"\n{stringBuilder.ToString().Indent(indentSize, indentToken)}\n";
            }

            Console.WriteLine(logMessage);
        }
    }
}
