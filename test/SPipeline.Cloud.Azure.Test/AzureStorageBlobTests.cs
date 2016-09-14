namespace SPipeline.Cloud.Azure.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SPipeline.Cloud.Azure.StorageBlob;
    using SPipeline.Core.Models;
    using SPipeline.Logger.NLog;
    using SPipeline.Pipeline;
    using System;

    [TestClass]
    public class AzureStorageBlobTests
    {
        [TestMethod]
        [Ignore]
        [TestCategory("Integration"), TestCategory("AzureQueue")]
        public void AzureStorageBlobQueue_SendAndReceiveMessage()
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

            var azureBlobSendConfiguration
                = new AzureStorageBlobSenderConfiguration
                {
                    ConnectionString = connectionString,
                    QueueName = queueName,
                    CreateQueue = true
                };

            var azureBlobReceiverConfiguration
                = new AzureStorageBlobReceiverConfiguration
                {
                    ConnectionString = connectionString,
                    QueueName = queueName,
                    CreateQueue = false
                };

            try
            {
                var sender = new AzureStorageBlobSender(azureBlobSendConfiguration, loggerService);
                sender.Send<MyMessageResponse>(message);
                sender.Send<MyMessageResponse>(message);

                var messageDispatcher = new MessageDispatcher().RegisterPipeline(genericPipeline);
                var receiver = new AzureStorageBlobReceiver(azureBlobReceiverConfiguration, messageDispatcher, loggerService);
                receiver.Process();
            }
            catch (Exception ex)
            {
                loggerService.Exception(ex);
            }
        }
    }
}
