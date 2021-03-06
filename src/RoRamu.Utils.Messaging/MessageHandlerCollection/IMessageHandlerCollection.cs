namespace RoRamu.Utils.Messaging
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for representing a collection of message handlers.
    /// </summary>
    public interface IMessageHandlerCollection
    {
        /// <summary>
        /// The list of currently mapped message types.
        /// </summary>
        /// <value>The list of currently mapped message types.</value>
        IEnumerable<string> MappedMessageTypes { get; }

        /// <summary>
        /// Handles a message with the mapped handler for the message type.
        /// If no handler is found, the default message handler is used, which throws an
        /// <see cref="RoRamu.Utils.Messaging.UnknownMessageTypeException"/>.
        /// </summary>
        /// <param name="message">The message to handle.</param>
        Task HandleMessage(Message message);
    }
}