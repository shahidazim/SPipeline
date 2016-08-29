﻿namespace SPipeline.Cloud.Azure.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SPipeline.Cloud.Azure.Blob;
    using SPipeline.Core.Models;
    using SPipeline.Pipeline;

    [TestClass]
    public class AzureBlobQueueTests
    {
        [TestMethod]
        [Ignore]
        [TestCategory("Integration"), TestCategory("AzureQueue")]
        public void AzureBlobQueue_SendAndReceiveMessage()
        {
            var message = new MyMessageRequest(new PipelineConfiguration
            {
                ClearErrorsBeforeNextHandler = false,
                BatchSizeForParallelHandlers = 10
            })
            {
                Name = "Hello World!"
            };

            var genericPipeline = new GenericPipeline<MyMessageRequest, MyMessageResponse>();

            var connectionString = "<connection-string>";
            var queueName = "<queue-name>";

            var azureBlobSendConfiguration
                = new AzureBlobSenderConfiguration
                {
                    ConnectionString = connectionString,
                    QueueName = queueName,
                    CreateQueue = true
                };

            var azureBlobReceiverConfiguration
                = new AzureBlobReceiverConfiguration
                {
                    ConnectionString = connectionString,
                    QueueName = queueName,
                    MessageReceiveThreadTimeoutMilliseconds = 1000,
                    CreateQueue = false
                };

            var sender = new AzureBlobSender(azureBlobSendConfiguration);
            sender.Send<MyMessageResponse>(message);
            sender.Send<MyMessageResponse>(message);

            var messageDispatcher = new MessageDispatcher().RegisterPipeline(genericPipeline);
            var receiver = new AzureBlobReceiver(azureBlobReceiverConfiguration, messageDispatcher);
            receiver.Start();
        }
    }
}