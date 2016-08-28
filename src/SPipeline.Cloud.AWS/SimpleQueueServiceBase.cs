namespace SPipeline.Cloud.AWS
{
    using Amazon.Runtime;
    using Amazon.SQS;
    using Amazon.SQS.Model;

    public abstract class SimpleQueueServiceBase
    {
        protected readonly string serviceUrl;
        protected readonly string accountId;
        protected readonly string queueName;
        protected AmazonSQSClient queueClient;

        protected SimpleQueueServiceBase(string serviceUrl, string queueName, string accountId, string accessKey, string secretKey, bool createQueue)
        {
            this.serviceUrl = serviceUrl;
            this.accountId = accountId;
            this.queueName = queueName;

            var awsCredentials = new BasicAWSCredentials(accessKey, secretKey);
            var sqsConfig = new AmazonSQSConfig
            {
                ServiceURL = serviceUrl
            };
            queueClient = new AmazonSQSClient(awsCredentials, sqsConfig);
            if (createQueue)
            {
                CreateQueue(queueName);
            }
        }

        private void CreateQueue(string queueName)
        {
            queueClient.CreateQueue(new CreateQueueRequest(queueName));
        }
    }
}
