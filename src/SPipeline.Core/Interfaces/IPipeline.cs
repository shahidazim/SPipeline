namespace SPipeline.Core.Interfaces
{
    using System;

    /// <summary>
    /// Represents the generic pipeline.
    /// </summary>
    /// <typeparam name="TMessageRequest">The type of the message request.</typeparam>
    /// <typeparam name="TMessageResponse">The type of the message response.</typeparam>
    public interface IPipeline<in TMessageRequest, out TMessageResponse>
        where TMessageRequest : IMessageRequest
        where TMessageResponse : IMessageResponse
    {
        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        Type MessageType { get; }

        TMessageResponse Execute(TMessageRequest messageRequest);
    }

    /// <summary>
    /// Represents the pipeline.
    /// </summary>
    public interface IPipeline
    {
        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        Type MessageType { get; }

        /// <summary>
        /// Executes the specified message request.
        /// </summary>
        /// <param name="messageRequest">The message request.</param>
        /// <returns></returns>
        IMessageResponse Execute(IMessageRequest messageRequest);
    }
}
