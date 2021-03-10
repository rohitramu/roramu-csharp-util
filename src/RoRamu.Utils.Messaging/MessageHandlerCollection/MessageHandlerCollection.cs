namespace RoRamu.Utils.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal class MessageHandlerCollection : Dictionary<string, MessageHandlerCollectionBuilder.HandlerAsyncDelegate>, IMessageHandlerCollection
    {
        public IEnumerable<string> MappedMessageTypes => this.Keys;

        /// <summary>
        /// The default message handler to use if one hasn't been mapped for the provided message type.
        /// </summary>
        private static MessageHandlerCollectionBuilder.HandlerAsyncDelegate DefaultFallbackMessageHandler { get; } = (message) =>
        {
            throw new UnknownMessageTypeException(message.Type);
        };

        internal MessageHandlerCollectionBuilder.HandlerAsyncDelegate FallbackMessageHandler { get; set; }

        /// <summary>
        /// Handles a message with the mapped handler if one is available, otherwise uses the
        /// default handler.
        /// </summary>
        /// <param name="message">The message to handle.</param>
        public async Task HandleMessage(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (!this.TryGetValue(message.Type, out MessageHandlerCollectionBuilder.HandlerAsyncDelegate handler))
            {
                handler = FallbackMessageHandler ?? DefaultFallbackMessageHandler;
            }

            await handler(message);
        }
    }
}