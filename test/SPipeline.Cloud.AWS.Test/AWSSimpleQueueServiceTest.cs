namespace SPipeline.Cloud.AWS.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SPipeline.Core.Models;
    using SPipeline.Cloud.AWS;
    using SPipeline.Pipeline;

    [TestClass]
    public class AWSSimpleQueueServiceTest
    {
        public class MyMessage : MessageRequestBase
        {
            public MyMessage(PipelineConfiguration configuration) : base(configuration)
            {
            }

            public string Name { get; set; }
        }

        public class MyConfiguration
        {
        }

        [TestMethod]
        [TestCategory("Integration"), TestCategory("SQS")]
        public void AWSSimpleQueueService_SendMessage()
        {
            var message = new MyMessage(new PipelineConfiguration { ClearErrorsBeforeNextHandler = false })
            {
                Name = "Hello World!"
            };
            var sender = new SimpleQueueServiceSender(new SimpleQueueServiceSenderConfiguration
            {
                ServiceUrl = "<service-url>",
                AccountId = "<account-id>",
                QueueName = "<queue-name>",
                AccessKey = "<access-key>",
                SecretKey = "<secret-key>"
            });
            sender.Send(message);
        }
    }
}
