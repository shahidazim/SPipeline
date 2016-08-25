namespace SPipeline.Cloud.AWS
{
    using Amazon.Runtime;
    using Amazon.SQS;
    using Amazon.SQS.Model;

    public abstract class SimpleQueueServiceBase
    {
        protected readonly string ServiceUrl;
        protected readonly string AccountId;
        protected readonly string QueueName;
        protected AmazonSQSClient QueueClient;

        protected SimpleQueueServiceBase(string serviceUrl, string queueName, string accountId, string accessKey, string secretKey)
        {
            ServiceUrl = serviceUrl;
            AccountId = accountId;
            QueueName = queueName;

            var awsCredentials = new BasicAWSCredentials(accessKey, secretKey);
            var sqsConfig = new AmazonSQSConfig
            {
                ServiceURL = serviceUrl
            };
            QueueClient = new AmazonSQSClient(awsCredentials, sqsConfig);
            CreateQueue(queueName);
        }

        private void CreateQueue(string queueName)
        {
            QueueClient.CreateQueue(new CreateQueueRequest(queueName));
        }
    }
}
