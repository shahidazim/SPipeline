﻿namespace SPipeline.Cloud.AWS.SQS
{
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Serializers;
    using System;

    public class AWSSQSReceiver : AWSSQSBase, IMessageReceiver
    {
        private readonly IMessageDispatcher _messageDispatcher;

        public AWSSQSReceiver(
            AWSSQSReceiverConfiguration configuration,
            IMessageDispatcher messageDispatcher,
            ILoggerService loggerService)
            : base(
                  configuration.ServiceUrl,
                  configuration.QueueName,
                  configuration.AccountId,
                  configuration.AccessKey,
                  configuration.SecretKey,
                  configuration.CreateQueue,
                  loggerService,
                  configuration.MaxNumberOfMessages)
        {
            _messageDispatcher = messageDispatcher;
        }

        /// <summary>
        /// Receiver messages from queue and process them
        /// </summary>
        public void Process()
        {
            var receivedMessages = queueService.Receive();

            try
            {
                foreach (var receivedMessage in receivedMessages)
                {
                    var message = GetBody(receivedMessage);
                    var response = _messageDispatcher.Execute(message);

                    if (response.HasError)
                    {
                        loggerService?.Error(response.GetFormattedError());
                    }
                }
            }
            catch (Exception ex)
            {
                loggerService?.Exception(ex);
            }
        }

        private IMessageRequest GetBody(string message)
        {
            return (IMessageRequest)SerializerJson.Deserialize(message);
        }
    }
}
