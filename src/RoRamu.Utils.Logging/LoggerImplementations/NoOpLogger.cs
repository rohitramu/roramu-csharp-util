namespace RoRamu.Utils.Logging
{
    public class NoOpLogger : Logger
    {
        protected override void HandleLog<T>(
            LogLevel logLevel,
            string message,
            T extraInfo,
            string callerName,
            string sourceFilePath,
            int sourceLineNumber)
        { }
    }
}
