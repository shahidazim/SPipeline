﻿namespace SPipeline.Cloud.Azure.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SPipeline.Cloud.Azure.ServiceBusQueue;
    using SPipeline.Core.Models;
    using SPipeline.Logger.NLog;
    using SPipeline.Pipeline;
    using System;

    [TestClass]
    public class AzureServiceBusQueueTests
    {
        [TestMethod]
        [Ignore]
        [TestCategory("Integration"), TestCategory("AzureQueue")]
        public void AzureServiceBusQueue_SendAndReceiveMessage()
        {
            var loggerService = new LoggerService("Azure");
            var message = new MyMessageRequest(new PipelineConfiguration
            {
                ClearErrorsBeforeNextHandler = false,
                BatchSizeForParallelHandlers = 10
            })
            {
                Name = "Hello World!"
            };

            var genericPipeline = new GenericPipeline<MyMessageRequest, MyMessageResponse>(loggerService);

            const string connectionString = "<connection-string>";
            const string queueName = "<queue-name>";

            var azureServiceBusSendConfiguration
                = new AzureServiceBusQueueSenderConfiguration
                {
                    ConnectionString = connectionString,
                    QueueName = queueName,
                    MaxSizeInMegabytes = 10240,
                    MessageTimeToLive = new TimeSpan(1, 0, 0, 0),
                    CreateQueue = true
                };

            var azureServiceBusReceiverConfiguration
                = new AzureServiceBusQueueReceiverConfiguration
                {
                    ConnectionString = connectionString,
                    QueueName = queueName,
                    MaxNumberOfMessages = 10,
                    CreateQueue = false
                };

            try
            {
                var sender = new AzureServiceBusQueueSender(azureServiceBusSendConfiguration, loggerService);
                sender.Send<MyMessageResponse>(message);
                sender.Send<MyMessageResponse>(message);

                var messageDispatcher = new MessageDispatcher().RegisterPipeline(genericPipeline);
                var receiver = new AzureServiceBusQueueReceiver(azureServiceBusReceiverConfiguration, messageDispatcher, loggerService);
                receiver.Process();
            }
            catch (Exception ex)
            {
                loggerService.Exception(ex);
            }
        }
    }
}
