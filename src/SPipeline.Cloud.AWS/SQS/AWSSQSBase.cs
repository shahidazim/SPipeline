namespace SPipeline.Cloud.AWS.SQS
{
    using SPipeline.Cloud.AWS.Services;
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Services;

    public abstract class AWSSQSBase
    {
        protected readonly ILoggerService loggerService;
        protected readonly IQueueService queueService;

        protected AWSSQSBase(string serviceUrl, string queueName, string accountId, string accessKey, string secretKey, bool createQueue, ILoggerService loggerService, int maxNumberOfMessages = 0)
        {
            this.loggerService = loggerService;
            queueService = new AWSSQSService(serviceUrl, queueName, accountId, accessKey, secretKey, createQueue, maxNumberOfMessages);
        }
    }
}
