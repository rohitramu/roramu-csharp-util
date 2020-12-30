namespace RoRamu.Utils
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

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
        public static string ToJsonString(this Exception exception, bool includeExceptionType = false, bool includeStackTrace = false)
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

            object obj;
            if (includeExceptionType || includeStackTrace)
            {
                obj = new
                {
                    type = includeExceptionType ? exception.GetType().FullName : null,
                    message = exception.Message,
                    innerMessages = messagesList,
                    stackTrace = includeStackTrace ? exception.StackTrace.Replace("\r", string.Empty).Split('\n') : null,
                };
            }
            else
            {
                obj = new
                {
                    message = exception.Message,
                    innerMessages = messagesList,
                };
            }

            string result = JsonConvert.SerializeObject(obj);

            return result;
        }
    }
}
