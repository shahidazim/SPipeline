namespace SPipeline.Cloud.Azure.Blob
{
    using SPipeline.Cloud.Azure.Services;
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Services;

    public abstract class AzureBlobBase
    {
        protected readonly IStorageService storageService;
        protected readonly ILoggerService loggerService;

        protected AzureBlobBase(string connectionString, string queueName, bool createQueue, ILoggerService loggerService)
        {
            storageService = new AzureBlobService(connectionString, queueName, createQueue);
            this.loggerService = loggerService;
        }
    }
}
