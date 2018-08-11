namespace RoRamu.Utils.Logging
{
    using System;
    using System.Runtime.CompilerServices;
    using RoRamu.Utils;

    public class ConsoleLogger : Logger
    {
        protected override void HandleLog<T>(
            LogLevel logLevel,
            string message,
            T extraInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            string logMessage = $"[{logLevel.ToString()}]".PadRight(9);
            logMessage += $" '{memberName}' in '{sourceFilePath}' (line {sourceLineNumber})";
            if (message != null)
            {
                logMessage += $"\n{message.Indent()}";
            }
            if (extraInfo != null)
            {
                logMessage += "\n--";
                logMessage += $"\n{extraInfo.ToString().Indent()}";
                logMessage += "\n--\n";
            }

            Console.WriteLine(logMessage);
        }
    }
}
