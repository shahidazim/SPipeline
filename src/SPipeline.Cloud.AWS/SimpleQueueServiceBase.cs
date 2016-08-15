namespace SPipeline.Cloud.AWS
{
    using Amazon.SQS;
    using SPipeline.Core.Interfaces;
    using System.Xml;

    public abstract class SimpleQueueServiceBase : IMessageSender
    {
        protected AmazonSQSClient QueueClient;

        protected SimpleQueueServiceBase()
        {
            var sqsConfig = new AmazonSQSConfig
            {
                ServiceURL = "https://eu-west-1.console.aws.amazon.com"
            };

            QueueClient = new AmazonSQSClient(sqsConfig);
        }

        public void Send(IMessageRequest message)
        {
            throw new System.NotImplementedException();
        }

        public void Send(XmlDocument message)
        {
            throw new System.NotImplementedException();
        }
    }
}
