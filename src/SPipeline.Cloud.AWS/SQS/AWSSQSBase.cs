namespace SPipeline.Cloud.AWS.SQS
{
    using Amazon.Runtime;
    using Amazon.SQS;
    using Amazon.SQS.Model;
    using SPipeline.Core.DebugHelper;

    public abstract class AWSSQSBase
    {
        protected readonly string serviceUrl;
        protected readonly string accountId;
        protected readonly string queueName;
        protected AmazonSQSClient queueClient;
        protected readonly ILoggerService loggerService;

        protected AWSSQSBase(string serviceUrl, string queueName, string accountId, string accessKey, string secretKey, bool createQueue, ILoggerService loggerService)
        {
            this.serviceUrl = serviceUrl;
            this.accountId = accountId;
            this.queueName = queueName;
            this.loggerService = loggerService;

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
