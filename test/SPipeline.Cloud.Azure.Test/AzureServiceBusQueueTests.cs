namespace SPipeline.Cloud.Azure.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SPipeline.Cloud.Azure.ServiceBus;
    using SPipeline.Core.Models;
    using SPipeline.Logger.NLog;
    using SPipeline.Pipeline;
    using System;

    public class MyMessageRequest : MessageRequestBase
    {
        public MyMessageRequest(PipelineConfiguration configuration) : base(configuration)
        {
        }

        public string Name { get; set; }
    }

    public class MyMessageResponse : MessageResponseBase
    {
    }

    public class MyConfiguration
    {
    }

    [TestClass]
    public class AzureServiceBusQueueTests
    {
        [TestMethod]
        [Ignore]
        [TestCategory("Integration"), TestCategory("AzureQueue")]
        public void AzureServiceBusQueue_SendAndReceiveMessage()
        {
            var message = new MyMessageRequest(new PipelineConfiguration
            {
                ClearErrorsBeforeNextHandler = false,
                BatchSizeForParallelHandlers = 10
            })
            {
                Name = "Hello World!"
            };

            var genericPipeline = new GenericPipeline<MyMessageRequest, MyMessageResponse>(new LoggerService("Generic"));

            var connectionString = "<connection-string>";
            var queueName = "<queue-name>";

            var azureServiceBusSendConfiguration
                = new AzureServiceBusSenderConfiguration
                {
                    ConnectionString = connectionString,
                    QueueName = queueName,
                    MaxSizeInMegabytes = 10240,
                    MessageTimeToLive = new TimeSpan(1, 0, 0, 0),
                    CreateQueue = true
                };

            var azureServiceBusReceiverConfiguration
                = new AzureServiceBusReceiverConfiguration
                {
                    ConnectionString = connectionString,
                    QueueName = queueName,
                    MessageReceiveThreadTimeoutMilliseconds = 1000,
                    MaxNumberOfMessages = 10,
                    CreateQueue = false
                };

            var loggerService = new LoggerService("Azure");
            try
            {
                var sender = new AzureServiceBusSender(azureServiceBusSendConfiguration, loggerService);
                sender.Send<MyMessageResponse>(message);
                sender.Send<MyMessageResponse>(message);

                var messageDispatcher = new MessageDispatcher().RegisterPipeline(genericPipeline);
                var receiver = new AzureServiceBusReceiver(azureServiceBusReceiverConfiguration, messageDispatcher, loggerService);
                receiver.Start();
            }
            catch (Exception ex)
            {
                loggerService.Exception(ex);
            }
        }
    }
}
