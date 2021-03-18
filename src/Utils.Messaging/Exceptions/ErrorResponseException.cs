namespace RoRamu.Utils.Messaging
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents an error response.
    /// </summary>
    public class ErrorResponseException : Exception
    {
        /// <summary>
        /// The error information.
        /// </summary>
        public SerializableExceptionInfo ErrorInfo { get; }

        /// <summary>
        /// Creates a new <see cref="ErrorResponseException" />.
        /// </summary>
        /// <param name="errorInfo">The error information.</param>
        public ErrorResponseException(SerializableExceptionInfo errorInfo) : base()
        {
            this.ErrorInfo = errorInfo;
        }

        /// <summary>
        /// Creates a new <see cref="ErrorResponseException" />.
        /// </summary>
        /// <param name="errorInfo">The error information.</param>
        /// <param name="info">The serialization information.</param>
        /// <param name="context">The streaming context.</param>
        protected ErrorResponseException(SerializableExceptionInfo errorInfo, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.ErrorInfo = errorInfo;
        }

        /// <summary>
        /// Creates a new <see cref="ErrorResponseException" />.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorInfo">The error information.</param>
        public ErrorResponseException(string message, SerializableExceptionInfo errorInfo) : base(message)
        {
            this.ErrorInfo = errorInfo;
        }

        /// <summary>
        /// Creates a new <see cref="ErrorResponseException" />.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorInfo">The error information.</param>
        /// <param name="innerException">The inner exception.</param>
        public ErrorResponseException(string message, SerializableExceptionInfo errorInfo, Exception innerException) : base(message, innerException)
        {
            this.ErrorInfo = errorInfo;
        }
    }
}