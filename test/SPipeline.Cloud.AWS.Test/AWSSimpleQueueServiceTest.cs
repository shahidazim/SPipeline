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
        }

        public class MyConfiguration
        {
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void AWSSimpleQueueService_SendMessage()
        {
            var message = new MyMessage(new PipelineConfiguration
            {
                ClearErrorsBeforeNextHandler = false
            });
            var sender = new SimpleQueueServiceSender();
            sender.Send(message);
        }
    }
}
