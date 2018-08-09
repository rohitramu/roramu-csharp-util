namespace RoRamu.Utils
{
    using System;

    public static class ExceptionUtils
    {
        public static string ToJsonString(this Exception exception, bool includeStackTraceAndExceptionType = false)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            object obj;
            if (includeStackTraceAndExceptionType)
            {
                obj = new
                {
                    Type = exception.GetType().FullName,
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                };
            }
            else
            {
                obj = exception.Message;
            }

            string result = JsonUtils.SerializeToJson(obj);

            return result;
        }
    }
}
