namespace RoRamu.Utils
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Stores the properties of an <see cref="Exception" /> object in a way that is easy to serialize.
    /// </summary>
    public class SerializableExceptionInfo
    {
        /// <summary>
        /// The type name of the exception.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// The message in the exception.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// The messages from inner exceptions, ordered from the outer-most (current) exception to the inner-most exception.
        /// </summary>
        public IEnumerable<string> InnerMessages { get; }

        /// <summary>
        /// The stack trace.  Each entry represents a single frame.
        /// </summary>
        public IEnumerable<string> StackTrace { get; }

        /// <summary>
        /// Creates a <see cref="SerializableExceptionInfo" /> instance.
        /// </summary>
        /// <param name="exception">The exception to extract/format information from.</param>
        /// <param name="includeExceptionType">Includes the type name of the exception.</param>
        /// <param name="includeStackTrace">Includes the stack trace in the resulting string.</param>
        public SerializableExceptionInfo(Exception exception, bool includeExceptionType = false, bool includeStackTrace = false)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            IList<string> messagesList = new List<string>();
            Exception current = exception.InnerException;
            while (current != null)
            {
                messagesList.Add(current.Message);
                current = current.InnerException;
            }

            this.Type = includeExceptionType ? exception.GetType().FullName : null;
            this.Message = exception.Message;
            this.InnerMessages = messagesList;
            this.StackTrace = includeStackTrace ? exception.StackTrace.Replace("\r", string.Empty).Split('\n') : null;
        }
    }
}