namespace SPipeline.Cloud.AWS
{
    using Amazon.Runtime;
    using Amazon.SQS;
    using Amazon.SQS.Model;
    using Newtonsoft.Json;
    using SPipeline.Core.Interfaces;
    using System.Xml;

    public abstract class SimpleQueueServiceBase : IMessageSender
    {
        private readonly string _serviceUrl;
        private readonly string _accountId;
        private readonly string _queueName;

        protected AmazonSQSClient QueueClient;

        protected SimpleQueueServiceBase(string serviceUrl, string queueName, string accountId, string accessKey, string secretKey)
        {
            _serviceUrl = serviceUrl;
            _accountId = accountId;
            _queueName = queueName;

            var awsCredentials = new BasicAWSCredentials(accessKey, secretKey);
            var sqsConfig = new AmazonSQSConfig
            {
                ServiceURL = serviceUrl
            };
            QueueClient = new AmazonSQSClient(awsCredentials, sqsConfig);
            CreateQueue(queueName);
        }

        public void Send(IMessageRequest message)
        {
            QueueClient.SendMessage(new SendMessageRequest
            {
                MessageBody = JsonConvert.SerializeObject(message),
                QueueUrl = CreateQueueUrl()
            });
        }

        public void Send(XmlDocument message)
        {
            throw new System.NotImplementedException();
        }

        private void CreateQueue(string queueName)
        {
            QueueClient.CreateQueue(new CreateQueueRequest(queueName));
        }

        private string CreateQueueUrl()
        {
            var serviceUrl = _serviceUrl;
            if (_serviceUrl.EndsWith("/"))
            {
                serviceUrl = _serviceUrl.Substring(0, _serviceUrl.Length - 1);
            }
            return $"{serviceUrl}/{_accountId}/{_queueName}";
        }
    }
}
