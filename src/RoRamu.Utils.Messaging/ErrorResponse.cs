namespace RoRamu.Utils.Messaging
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using RoRamu.Utils;

    /// <summary>
    /// Represents a message that is sent in response to another message.  Objects of this type
    /// would indicate that there was an error in processing the request message.
    /// </summary>
    public class ErrorResponse : Response
    {
        /// <summary>
        /// The message type.
        /// </summary>
        public new const string MessageType = WellKnownMessageTypes.Error;

        /// <summary>
        /// The error.
        /// </summary>
        public SerializableExceptionInfo Error => this.ErrorInternal.Value;
        private Lazy<SerializableExceptionInfo> ErrorInternal { get; }

        /// <summary>
        /// Creates a new error response message.
        /// </summary>
        /// <param name="error">The exception representing the error.</param>
        /// <param name="requestId">The ID of the request message.</param>
        /// <param name="includeStackTrace">
        /// Whether or not to include the stack trace in the response message.
        /// </param>
        public ErrorResponse(
            Exception error,
            string requestId,
            bool includeStackTrace = false)
            : base(
                  requestId,
                  body: new JRaw(error == null
                    ? throw new ArgumentNullException(nameof(error))
                    : JsonConvert.SerializeObject(
                        value: error.ToSerializableExceptionInfo(includeExceptionType: true, includeStackTrace: includeStackTrace),
                        formatting: Formatting.Indented)),
                  isError: true)
        {
            this.ErrorInternal = new Lazy<SerializableExceptionInfo>(() => this.GetBody<SerializableExceptionInfo>());
        }

        private ErrorResponse(string requestId, SerializableExceptionInfo exceptionInfo) : base(requestId, exceptionInfo, isError: true)
        {
            this.ErrorInternal = new Lazy<SerializableExceptionInfo>(() => exceptionInfo);
        }

        /// <summary>
        /// Creates a new object which represents an error response message.
        /// </summary>
        /// <param name="error">The exception representing the error.</param>
        /// <param name="request">The request message.</param>
        /// <param name="includeDebugInfo">
        /// Whether or not to include the stack trace in the response message.
        /// </param>
        /// <returns>The error response message.</returns>
        public static ErrorResponse Create(Message request, Exception error, bool includeDebugInfo = false)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            return new ErrorResponse(error, request.Id, includeDebugInfo);
        }

        /// <summary>
        /// If the given message is an error response, converts it to an <see cref="ErrorResponse"/>
        /// object.
        /// </summary>
        /// <param name="message">The message to convert.</param>
        /// <param name="errorResponse">The converted error response.</param>
        /// <returns>True if parsing was successful, otherwise false.</returns>
        public static bool TryParse(Message message, out ErrorResponse errorResponse)
        {
            // Check message type
            if (message.Type != ErrorResponse.MessageType)
            {
                errorResponse = null;
                return false;
            }

            // Check the body
            message.TryGetBody(out SerializableExceptionInfo serializableExceptionInfo);

            // Return the new error response
            errorResponse = new ErrorResponse(message.Id, serializableExceptionInfo);
            return true;
        }
    }
}
