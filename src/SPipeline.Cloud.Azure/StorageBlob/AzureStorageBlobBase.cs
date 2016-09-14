namespace SPipeline.Cloud.Azure.StorageBlob
{
    using SPipeline.Cloud.Azure.Services;
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Services;

    public abstract class AzureStorageBlobBase
    {
        protected readonly IStorageService storageService;
        protected readonly ILoggerService loggerService;

        protected AzureStorageBlobBase(
            string connectionString,
            string queueName,
            bool createQueue,
            ILoggerService loggerService)
        {
            storageService = new AzureStorageBlobService(connectionString, queueName, createQueue);
            this.loggerService = loggerService;
        }
    }
}
