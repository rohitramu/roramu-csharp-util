namespace RoRamu.Utils
{
    using System;
    using Newtonsoft.Json;

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
