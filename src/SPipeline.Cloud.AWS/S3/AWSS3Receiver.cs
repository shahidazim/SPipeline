namespace SPipeline.Cloud.AWS.S3
{
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Serializers;
    using System;

    public class AWSS3Receiver : AWSS3Base, IMessageReceiver
    {
        private readonly IMessageDispatcher _messageDispatcher;

        public AWSS3Receiver(AWSS3ReceiverConfiguration configuration, IMessageDispatcher messageDispatcher, ILoggerService loggerService)
            : base(configuration.ServiceUrl, configuration.BucketName, configuration.AccessKey, configuration.SecretKey, configuration.CreateBucket, loggerService)
        {
            _messageDispatcher = messageDispatcher;
        }

        public void Process()
        {
            var objectKeys = s3StorageService.GetAllObjectKeys();
            foreach (var objectKey in objectKeys)
            {
                try
                {
                    var message = GetBody(objectKey);
                    var response = _messageDispatcher.Execute(message);

                    if (response.HasError)
                    {
                        loggerService?.Error(response.GetFormattedError());
                    }
                    else
                    {
                        s3StorageService.DeleteObject(objectKey);
                    }
                }
                catch (Exception ex)
                {
                    loggerService?.Exception(ex);
                }
            }
        }

        private IMessageRequest GetBody(string objectKey)
        {
            return (IMessageRequest)SerializerJson.Deserialize(s3StorageService.DownloadObject(objectKey));
        }
    }
}
