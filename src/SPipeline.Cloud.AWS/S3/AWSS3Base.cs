namespace SPipeline.Cloud.AWS.S3
{
    using SPipeline.Cloud.AWS.Services;
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Services;

    public abstract class AWSS3Base
    {
        protected readonly IStorageService storageService;
        protected readonly ILoggerService loggerService;

        protected AWSS3Base(string serviceUrl, string bucketName, string accessKey, string secretKey, bool createQueue, ILoggerService loggerService)
        {
            this.loggerService = loggerService;
            storageService = new AWSS3Service(serviceUrl, bucketName, accessKey, secretKey, createQueue);
        }
    }
}
