namespace RoRamu.Utils
{
    using System;

    /// <summary>
    /// Utility methods to handle exception objects.
    /// </summary>
    public static class ExceptionUtils
    {
        /// <summary>
        /// Serializes an exception to a JSON string suitable for error messages.
        /// </summary>
        /// <param name="exception">The exception to serialize.</param>
        /// <param name="includeExceptionType">Includes the type name of the exception.</param>
        /// <param name="includeStackTrace">Includes the stack trace in the resulting string.</param>
        /// <returns>The JSON string representation of the given exception object.</returns>
        public static SerializableExceptionInfo ToSerializableExceptionInfo(this Exception exception, bool includeExceptionType = false, bool includeStackTrace = false)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            return new SerializableExceptionInfo(exception, includeExceptionType, includeStackTrace);
        }
    }
}
