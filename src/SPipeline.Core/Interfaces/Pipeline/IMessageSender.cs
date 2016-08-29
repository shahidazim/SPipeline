namespace SPipeline.Core.Interfaces.Pipeline
{
    /// <summary>
    /// Represents the message sender
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <typeparam name="TMessageResponse">The type of the message response.</typeparam>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        IMessageResponse Send<TMessageResponse>(IMessageRequest message)
            where TMessageResponse : IMessageResponse, new();
    }
}
