﻿namespace SPipeline.Cloud.Azure.StorageQueue
{
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Serializers;
    using System;

    /// <summary>
    /// The Azure Service Bus Queue Receiver to receive and deserialize the messages
    /// </summary>
    /// <seealso cref="Core.Interfaces.Pipeline.IMessageReceiver" />
    public class AzureStorageQueueReceiver : AzureStorageQueueBase, IMessageReceiver
    {
        private readonly IMessageDispatcher _messageDispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageQueueReceiver" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="messageDispatcher">The message dispatcher.</param>
        /// <param name="loggerService">The logger service.</param>
        public AzureStorageQueueReceiver(
            AzureStorageQueueReceiverConfiguration configuration,
            IMessageDispatcher messageDispatcher,
            ILoggerService loggerService)
            : base(
                  configuration.ConnectionString,
                  configuration.QueueName,
                  configuration.CreateQueue,
                  null,
                  configuration.MaxNumberOfMessages,
                  loggerService)
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
