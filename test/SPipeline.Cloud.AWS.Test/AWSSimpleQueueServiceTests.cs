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
        public void AWSSimpleQueueService_SendMessage()
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

            var simpleQueueServiceConfiguration
                = new SimpleQueueServiceConfiguration
                {
                    ServiceUrl = "<service-url>",
                    AccountId = "<account-id>",
                    QueueName = "<queue-name>",
                    AccessKey = "<access-key>",
                    SecretKey = "<secret-key>",
                    MessageReceiveThreadTimeoutMilliseconds = 1000,
                    MaxNumberOfMessages = 10
                };

            var sender = new SimpleQueueServiceSender(simpleQueueServiceConfiguration);
            var senderResponse = sender.Send<MyMessageResponse>(message);
            senderResponse = sender.Send<MyMessageResponse>(message);

            var messageDispatcher = new MessageDispatcher().RegisterPipeline(genericPipeline);
            var receiver = new SimpleQueueServiceReceiver<IMessageResponse>(simpleQueueServiceConfiguration, messageDispatcher);
            receiver.Start();
        }
    }
}
