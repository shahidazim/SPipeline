namespace SPipeline.Cloud.AWS.S3
{
    using SPipeline.Cloud.AWS.Services;
    using SPipeline.Core.Interfaces.Services;

    public abstract class AWSS3Base
    {
        protected readonly IAWSS3StorageService s3StorageService;

        protected AWSS3Base(string serviceUrl, string bucketName, string accessKey, string secretKey, bool createQueue)
        {
            s3StorageService = new AWSS3StorageService(serviceUrl, bucketName, accessKey, secretKey, createQueue);
        }
    }
}
