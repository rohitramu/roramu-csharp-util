namespace RoRamu.Utils
{
    using System;

    public static class ExceptionUtils
    {
        public static string ToMessageString(this Exception exception, bool includeStackTraceAndExceptionType = false)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            string result = includeStackTraceAndExceptionType
                ? exception.ToString()
                : exception.Message;

            return result;
        }
    }
}
