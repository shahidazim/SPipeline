namespace SPipeline.Core.Interfaces
{
    /// <summary>
    /// Represents the message dispatcher.
    /// </summary>
    public interface IMessageDispatcher
    {
        /// <summary>
        /// Registers the pipeline.
        /// </summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        IMessageDispatcher RegisterPipeline(IPipeline pipeline);

        /// <summary>
        /// Executes the specified message request.
        /// </summary>
        /// <param name="messageRequest">The message request.</param>
        /// <returns></returns>
        IMessageResponse Execute(IMessageRequest messageRequest);
    }
}
