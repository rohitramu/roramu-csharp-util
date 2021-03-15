namespace RoRamu.Utils.Messaging
{
    using System;

    /// <summary>
    /// Extension methods for <see cref="RequestResult" /> objects.
    /// </summary>
    public static class RequestResultExtensions
    {
        /// <summary>
        /// Throws an exception if the request failed.
        /// </summary>
        /// <param name="result"></param>
        public static void ThrowOnError(this RequestResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (!result.IsSuccessful)
            {
                throw result.Exception ?? new ErrorResponseException("Unexpected failure - no exception found in error response.", null);
            }
        }
    }
}
