namespace SPipeline.Cloud.Azure.StorageQueue
{
    using SPipeline.Cloud.Azure.Services;
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Services;
    using System;

    public class AzureStorageQueueBase
    {
        protected readonly IQueueService queueService;
        protected readonly ILoggerService loggerService;

        protected AzureStorageQueueBase(
            string connectionString,
            string queueName,
            bool createQueue,
            TimeSpan? messageTimeToLive,
            int maxNumberOfMessages,
            ILoggerService loggerService)
        {
            this.loggerService = loggerService;
            queueService = new AzureStorageQueueService(
                connectionString,
                queueName,
                createQueue,
                messageTimeToLive,
                maxNumberOfMessages);
        }
    }
}
