namespace RoRamu.Utils.Logging
{
    using System;
    using System.Collections.Concurrent;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    public abstract class Logger
    {
        private static readonly ConcurrentDictionary<string, Logger> _loggerCache = new ConcurrentDictionary<string, Logger>();

        public static LogLevel LogLevel { get; set; } = LogLevel.Info;

        public static bool LogExtraInfo { get; set; } = true;

        private static Logger _defaultLogger = Logger.GetLogger<ConsoleLogger>();
        public static Logger Default
        {
            get => _defaultLogger;
            set
            {
                _defaultLogger = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        public static Logger GetLogger<TLoggerType>() where TLoggerType : Logger, new()
        {
            Logger result = Logger._loggerCache.GetOrAdd(
                typeof(TLoggerType).AssemblyQualifiedName,
                name => new TLoggerType());

            return result;
        }

        /// <summary>
        /// Logs a message.
        /// Implementations should never throw exceptions in this method.
        /// Implementations should always be thread-safe.
        /// </summary>
        /// <typeparam name="T">The type of the extra info object</typeparam>
        /// <param name="logLevel">The log level</param>
        /// <param name="message">The message</param>
        /// <param name="extraInfo">The extra info object</param>
        protected abstract void HandleLog<T>(
            LogLevel logLevel,
            string message,
            T extraInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0) where T : class;

        public void Log<T>(
            LogLevel logLevel,
            string message,
            T extraInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            where T : class
        {
            if (logLevel >= LogLevel)
            {
                Task.Run(() =>
                {
                    this.HandleLog(
                        logLevel,
                        message,
                        LogExtraInfo ? extraInfo : null,
                        memberName,
                        sourceFilePath,
                        sourceLineNumber);
                }).ContinueWith(task =>
                {
                    // In order to swallow the exception, we have to access the Exception property so it doesn't get propogated
                    var ex = task.Exception;
                }, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        public void Log(
            LogLevel logLevel,
            string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            this.Log<object>(
                logLevel,
                message,
                null,
                memberName,
                sourceFilePath,
                sourceLineNumber);
        }
    }
}
