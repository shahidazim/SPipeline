namespace SPipeline.Cloud.Azure.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SPipeline.Cloud.Azure.StorageQueue;
    using SPipeline.Core.Models;
    using SPipeline.Logger.NLog;
    using SPipeline.Pipeline;
    using System;

    [TestClass]
    public class AzureStorageQueueTests
    {
        [TestMethod]
        [Ignore]
        [TestCategory("Integration"), TestCategory("AzureQueue")]
        public void AzureStorageQueue_SendAndReceiveMessage()
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

            var azureStorageSendConfiguration
                = new AzureStorageQueueSenderConfiguration
                {
                    ConnectionString = connectionString,
                    QueueName = queueName,
                    MaxSizeInMegabytes = 10240,
                    MessageTimeToLive = new TimeSpan(1, 0, 0, 0),
                    CreateQueue = true
                };

            var azureStorageReceiverConfiguration
                = new AzureStorageQueueReceiverConfiguration
                {
                    ConnectionString = connectionString,
                    QueueName = queueName,
                    CreateQueue = false,
                    MaxNumberOfMessages = 10
                };

            try
            {
                var sender = new AzureStorageQueueSender(azureStorageSendConfiguration, loggerService);
                sender.Send<MyMessageResponse>(message);
                sender.Send<MyMessageResponse>(message);

                var messageDispatcher = new MessageDispatcher().RegisterPipeline(genericPipeline);
                var receiver = new AzureStorageQueueReceiver(azureStorageReceiverConfiguration, messageDispatcher, loggerService);
                receiver.Process();
            }
            catch (Exception ex)
            {
                loggerService.Exception(ex);
            }
        }
    }
}
