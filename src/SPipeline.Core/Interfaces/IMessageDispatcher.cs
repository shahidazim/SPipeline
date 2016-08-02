namespace SPipeline.Core.Interfaces
{
    /// <summary>
    /// Represents the message dispatcher.
    /// </summary>
    public interface IMessageDispatcher
    {
        /// <summary>
        /// Executes the specified message request.
        /// </summary>
        /// <param name="messageRequest">The message request.</param>
        /// <returns></returns>
        IMessageResponse Execute(IMessageRequest messageRequest);
    }
}
