namespace SPipeline.Pipeline
{
    using SPipeline.Core.Exceptions;
    using SPipeline.Core.Interfaces;
    using System;
    using System.Collections.Generic;

    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly Dictionary<Type, IPipeline> _pipelines = new Dictionary<Type, IPipeline>();

        public MessageDispatcher RegisterPipeline(IPipeline pipeline)
        {
            _pipelines.Add(pipeline.MessageType, pipeline);
            return this;
        }

        public IMessageResponse Execute(IMessageRequest messageRequest)
        {
            var messageType = messageRequest.GetType();

            if (!_pipelines.ContainsKey(messageType))
            {
                throw new PipelineNotRegisteredException(messageType);
            }

            return _pipelines[messageType].Execute(messageRequest);
        }
    }
}
