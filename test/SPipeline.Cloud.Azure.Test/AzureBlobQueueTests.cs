namespace SPipeline.Cloud.Azure.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SPipeline.Cloud.Azure.Blob;
    using SPipeline.Core.Models;
    using SPipeline.Logger.NLog;
    using SPipeline.Pipeline;
    using System;

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

            var genericPipeline = new GenericPipeline<MyMessageRequest, MyMessageResponse>(new LoggerService("Azure"));

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
                    CreateQueue = false
                };

            var loggerService = new LoggerService("Azure");
            try
            {
                var sender = new AzureBlobSender(azureBlobSendConfiguration, loggerService);
                sender.Send<MyMessageResponse>(message);
                sender.Send<MyMessageResponse>(message);

                var messageDispatcher = new MessageDispatcher().RegisterPipeline(genericPipeline);
                var receiver = new AzureBlobReceiver(azureBlobReceiverConfiguration, messageDispatcher, loggerService);
                receiver.Process();
            }
            catch (Exception ex)
            {
                loggerService.Exception(ex);
            }
        }
    }
}
