namespace SPipeline.Core.Interfaces
{
    using System.Xml;

    /// <summary>
    /// Represents the message sender
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Send(IMessageRequest message);

        /// <summary>
        /// Sends the specified XML message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Send(XmlDocument message);
    }
}
