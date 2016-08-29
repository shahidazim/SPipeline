namespace SPipeline.Cloud.Azure.Blob
{
    using SPipeline.Cloud.Azure.Services;
    using SPipeline.Core.Interfaces.Services;

    public abstract class AzureBlobBase
    {
        protected readonly IBlobStorageService blobStorageService;

        protected AzureBlobBase(string connectionString, string queueName, bool createQueue)
        {
            blobStorageService = new AzureBlobStorageService(connectionString, queueName, createQueue);
        }
    }
}
