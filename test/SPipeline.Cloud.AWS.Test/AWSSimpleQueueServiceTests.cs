namespace SPipeline.Cloud.AWS.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SPipeline.Cloud.AWS;
    using SPipeline.Core.Models;
    using SPipeline.Core.Interfaces;
    using SPipeline.Pipeline;

    [TestClass]
    public class AWSSimpleQueueServiceTests
    {
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

        [TestMethod]
        [Ignore]
        [TestCategory("Integration"), TestCategory("SQS")]
        public void AWSSimpleQueueService_SendAndReceiveMessages()
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

            var serviceUrl = "<service-url>";
            var accountId = "<account-id>";
            var queueName = "<queue-name>";
            var accessKey = "<access-key>";
            var secretKey = "<secret-key>";


            var simpleQueueServiceSenderConfiguration
                = new SimpleQueueServiceSendConfiguration
                {
                    ServiceUrl = serviceUrl,
                    AccountId = accountId,
                    QueueName = queueName,
                    AccessKey = accessKey,
                    SecretKey = secretKey,
                    CreateQueue = true
                };

            var simpleQueueServiceReceiveConfiguration
                = new SimpleQueueServiceReceiverConfiguration
                {
                    ServiceUrl = serviceUrl,
                    AccountId = accountId,
                    QueueName = queueName,
                    AccessKey = accessKey,
                    SecretKey = secretKey,
                    MessageReceiveThreadTimeoutMilliseconds = 1000,
                    MaxNumberOfMessages = 10,
                    CreateQueue = false
                };

            var sender = new SimpleQueueServiceSender(simpleQueueServiceSenderConfiguration);
            sender.Send<MyMessageResponse>(message);
            sender.Send<MyMessageResponse>(message);

            var messageDispatcher = new MessageDispatcher().RegisterPipeline(genericPipeline);
            var receiver = new SimpleQueueServiceReceiver(simpleQueueServiceReceiveConfiguration, messageDispatcher);
            receiver.Start();
        }
    }
}
