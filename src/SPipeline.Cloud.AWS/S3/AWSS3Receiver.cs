namespace SPipeline.Cloud.AWS.S3
{
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Serializers;
    using SPipeline.Core.Services;
    using System;

    public class AWSS3Receiver : AWSS3Base
    {
        private readonly IMessageDispatcher _messageDispatcher;
        private readonly IMessageReceiver _messageReceiver;

        public AWSS3Receiver(AWSS3ReceiverConfiguration configuration, IMessageDispatcher messageDispatcher, IMessageReceiver messageReceiver, ILoggerService loggerService)
            : base(configuration.ServiceUrl, configuration.BucketName, configuration.AccessKey, configuration.SecretKey, configuration.CreateBucket, loggerService)
        {
            _messageDispatcher = messageDispatcher;
            _messageReceiver = messageReceiver;
            _messageReceiver.StartCallback = StartCallback;
        }

        public AWSS3Receiver(AWSS3ReceiverConfiguration configuration, IMessageDispatcher messageDispatcher, ILoggerService loggerService)
            : this(configuration, messageDispatcher, new GenericMessageReceiver(configuration.MessageReceiveThreadTimeoutMilliseconds), loggerService)
        {
        }

        public void Start()
        {
            _messageReceiver.Start();
        }

        public void Stop()
        {
            _messageReceiver.Stop();
        }

        /// <summary>
        /// Starts the callback.
        /// </summary>
        private void StartCallback()
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
