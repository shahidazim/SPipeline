namespace SPipeline.Cloud.Azure.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SPipeline.Core.Interfaces;
    using SPipeline.Core.Models;
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

            var genericPipeline = new GenericPipeline<MyMessageRequest, MyMessageResponse>();

            var connectionString = "<connection-string>";
            var queueName = "<queue-name>";

            var azureServiceBusSendConfiguration
                = new AzureServiceBusSenderConfiguration
                {
                    ConnectionString = connectionString,
                    QueueName = queueName,
                    MaxSizeInMegabytes = 10240,
                    MessageTimeToLive = new TimeSpan(1, 0, 0, 0)
                };

            var azureServiceBusReceiverConfiguration
                = new AzureServiceBusReceiverConfiguration
                {
                    ConnectionString = connectionString,
                    QueueName = queueName,
                    MessageReceiveThreadTimeoutMilliseconds = 1000,
                    MaxNumberOfMessages = 10
                };

            var sender = new AzureServiceBusSender(azureServiceBusSendConfiguration);
            sender.Send<MyMessageResponse>(message);
            sender.Send<MyMessageResponse>(message);

            var messageDispatcher = new MessageDispatcher().RegisterPipeline(genericPipeline);
            var receiver = new AzureServiceBusReceiver(azureServiceBusReceiverConfiguration, messageDispatcher);
            receiver.Start();
        }
    }
}
