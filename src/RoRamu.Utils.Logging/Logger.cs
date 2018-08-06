namespace RoRamu.Logging
{
    using System.Collections.Concurrent;

    public abstract class Logger
    {
        private static readonly ConcurrentDictionary<string, Logger> _loggerCache = new ConcurrentDictionary<string, Logger>();

        public static readonly Logger DefaultLogger = Logger.GetLogger<ConsoleLogger>();

        public static Logger GetLogger<TLoggerType>() where TLoggerType : Logger, new()
        {
            Logger result = Logger._loggerCache.GetOrAdd(
                typeof(TLoggerType).AssemblyQualifiedName,
                name => new TLoggerType());

            return result;
        }

        public static LogLevel LogLevel { get; set; } = LogLevel.Info;

        public static bool LogExtraInfo { get; set; } = true;

        /// <summary>
        /// Logs a message.
        /// Implementations should never throw exceptions in this method.
        /// Implementations should always be thread-safe.
        /// </summary>
        /// <typeparam name="T">The type of the extra info object</typeparam>
        /// <param name="logLevel">The log level</param>
        /// <param name="message">The message</param>
        /// <param name="extraInfo">The extra info object</param>
        protected abstract void HandleLog<T>(LogLevel logLevel, string message, T extraInfo) where T : class;

        public void Log<T>(LogLevel logLevel, string message, T extraInfo) where T : class
        {
            if (logLevel >= LogLevel)
            {
                this.HandleLog(
                    logLevel,
                    message,
                    LogExtraInfo ? extraInfo : null);
            }
        }

        public void Log(LogLevel logLevel, string message)
        {
            this.Log<object>(logLevel, message, null);
        }
    }
}
