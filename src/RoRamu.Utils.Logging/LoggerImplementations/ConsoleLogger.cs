namespace RoRamu.Logging
{
    using System;
    using System.Linq;
    using RoRamu.Utils;

    public class ConsoleLogger : Logger
    {
        protected override void HandleLog<T>(LogLevel logLevel, string message, T extraInfo)
        {
            int indent = 10;
            string prefix = $"[{logLevel.ToString()}]".PadRight(indent - 1);
            string logMessage = prefix;
            string[] splitMessage = message.Split("\n");
            logMessage += $" {splitMessage[0]}";
            if (splitMessage.Length > 1)
            {
                logMessage += $"\n{string.Join('\n', splitMessage.Skip(1)).Indent(indent, " ")}";
            }
            if (extraInfo != null)
            {
                logMessage += $"\n{extraInfo.ToString().Indent(indent, " ")}";
            }

            Console.WriteLine(logMessage);
        }
    }
}
