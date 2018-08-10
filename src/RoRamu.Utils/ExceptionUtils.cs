namespace RoRamu.Utils
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public static class ExceptionUtils
    {
        public static string ToJsonString(this Exception exception, bool includeStackTraceAndExceptionType = false)
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
            if (includeStackTraceAndExceptionType)
            {
                obj = new
                {
                    type = exception.GetType().FullName,
                    message = exception.Message,
                    innerMessages = messagesList,
                    stackTrace = exception.StackTrace.Replace("\r", string.Empty).Split('\n'),
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

        public static void WrapSafely(Action action, Action<Exception> onError = null)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            }
        }

        public static T WrapSafely<T>(Func<T> func, Action<Exception> onError = null)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
                return default(T);
            }
        }
    }
}
