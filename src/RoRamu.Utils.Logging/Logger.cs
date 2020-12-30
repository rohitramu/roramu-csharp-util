namespace RoRamu.Utils.Logging
{
    using System;
    using System.Collections.Concurrent;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    /// <summary>
    /// An abstract implementation of a logger.
    /// <para>
    /// All implementations must be thread safe, and have a default constructor, and should
    /// never throw exceptions in the overridden <see cref="HandleLog"/> method.
    /// </para>
    /// </summary>
    public abstract class Logger
    {
        private static readonly ConcurrentDictionary<string, Logger> _loggerCache = new ConcurrentDictionary<string, Logger>();

        /// <summary>
        /// The minimum severity to log across all loggers.
        /// This property provides an easy way to reduce the verbosity of all loggers at once.
        /// </summary>
        public static LogLevel GlobalLogLevel { get; set; } = LogLevel.Info;

        /// <summary>
        /// The minimum severity to log.
        /// </summary>
        public LogLevel LogLevel { get; set; } = LogLevel.Debug;

        /// <summary>
        /// The default logger (i.e. <see cref="ConsoleLogger"/>).
        /// </summary>
        public static ConsoleLogger Default => Logger.GetLogger<ConsoleLogger>();

        /// <summary>
        /// Gets an instance of the logger.
        /// </summary>
        /// <typeparam name="TLoggerType">The concrete logger implementation to use.</typeparam>
        /// <returns>The logger.</returns>
        public static TLoggerType GetLogger<TLoggerType>() where TLoggerType : Logger, new()
        {
            TLoggerType result = Logger._loggerCache.GetOrAdd(
                typeof(TLoggerType).AssemblyQualifiedName,
                name => new TLoggerType()) as TLoggerType;

            return result;
        }

        /// <summary>
        /// Logs a message.
        /// Implementations should never throw exceptions in this method.
        /// Implementations should always be thread-safe.
        /// </summary>
        /// <typeparam name="T">The type of the extra info object.</typeparam>
        /// <param name="logLevel">The log level.</param>
        /// <param name="message">The message.</param>
        /// <param name="extraInfo">The extra info object.</param>
        /// <param name="callerName">The calling method.</param>
        /// <param name="sourceFilePath">The path to the source file which contains the log statement.</param>
        /// <param name="sourceLineNumber">The line number of the log statement in the source file.</param>
        protected abstract void HandleLog<T>(
            LogLevel logLevel,
            string message,
            T extraInfo,
            string callerName,
            string sourceFilePath,
            int sourceLineNumber);

        /// <summary>
        /// Emits a log line with a message and the provided extra information.
        /// </summary>
        /// <param name="logLevel">The severity level of the log line.</param>
        /// <param name="message">The message to include in the log line.</param>
        /// <param name="extraInfo">Any extra information to include in the log line.</param>
        /// <param name="callerName">The calling method.</param>
        /// <param name="sourceFilePath">The path to the source file which contains the log statement.</param>
        /// <param name="sourceLineNumber">The line number of the log statement in the source file.</param>
        /// <typeparam name="T">The type of the object which contains the extra information to be included in the log line.</typeparam>
        public void Log<T>(
            LogLevel logLevel,
            string message,
            T extraInfo,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (logLevel >= GlobalLogLevel && logLevel >= this.LogLevel)
            {
                Task.Run(() =>
                {
                    try
                    {
                        this.HandleLog(
                            logLevel,
                            message,
                            extraInfo,
                            callerName,
                            sourceFilePath,
                            sourceLineNumber);
                    }
                    catch (Exception e)
                    {
                        // In order to swallow the exception, we have to access the Exception property so it doesn't get propogated
                        Console.WriteLine(e);
                    }
                });
            }
        }

        /// <summary>
        /// Emits a log line with a message.
        /// </summary>
        /// <param name="logLevel">The severity level of the log line.</param>
        /// <param name="message">The message to include in the log line.</param>
        /// <param name="callerName">The calling method.</param>
        /// <param name="sourceFilePath">The path to the source file which contains the log statement.</param>
        /// <param name="sourceLineNumber">The line number of the log statement in the source file.</param>
        public void Log(
            LogLevel logLevel,
            string message,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            this.Log<object>(
                logLevel,
                message,
                null,
                callerName,
                sourceFilePath,
                sourceLineNumber);
        }
    }
}
