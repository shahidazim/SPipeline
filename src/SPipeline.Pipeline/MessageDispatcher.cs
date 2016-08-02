namespace SPipeline.Pipeline
{
    using SPipeline.Core.Exceptions;
    using SPipeline.Core.Interfaces;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A utility class to register and execute pipeline based on the type of message.
    /// </summary>
    /// <seealso cref="SPipeline.Core.Interfaces.IMessageDispatcher" />
    public class MessageDispatcher : IMessageDispatcher
    {
        /// Represents the registered pipelines.
        private readonly Dictionary<Type, IPipeline> _pipelines = new Dictionary<Type, IPipeline>();

        /// <summary>
        /// Registers the pipeline.
        /// </summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public MessageDispatcher RegisterPipeline(IPipeline pipeline)
        {
            _pipelines.Add(pipeline.MessageType, pipeline);
            return this;
        }

        /// <summary>
        /// Executes the specified message request.
        /// </summary>
        /// <param name="messageRequest">The message request.</param>
        /// <returns></returns>
        /// <exception cref="SPipeline.Core.Exceptions.PipelineNotRegisteredException"></exception>
        public IMessageResponse Execute(IMessageRequest messageRequest)
        {
            // Get the type of message.
            var messageType = messageRequest.GetType();

            // Check if it is not registered, then throw exception.
            if (!_pipelines.ContainsKey(messageType))
            {
                throw new PipelineNotRegisteredException(messageType);
            }

            // Executes the specific pipeline.
            return _pipelines[messageType].Execute(messageRequest);
        }
    }
}
