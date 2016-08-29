namespace SPipeline.Cloud.AWS.S3
{
    using SPipeline.Core.Interfaces;
    using SPipeline.Core.Serializers;
    using SPipeline.Core.Services;
    using System;

    public class AWSS3Receiver : AWSS3Base
    {
        private readonly IMessageDispatcher _messageDispatcher;
        private readonly IMessageReceiver _messageReceiver;

        public AWSS3Receiver(AWSS3ReceiverConfiguration configuration, IMessageDispatcher messageDispatcher, IMessageReceiver messageReceiver)
            : base(configuration.ServiceUrl, configuration.BucketName, configuration.AccessKey, configuration.SecretKey, configuration.CreateBucket)
        {
            _messageDispatcher = messageDispatcher;
            _messageReceiver = messageReceiver;
            _messageReceiver.StartCallback = StartCallback;
        }

        public AWSS3Receiver(AWSS3ReceiverConfiguration configuration, IMessageDispatcher messageDispatcher)
            : this(configuration, messageDispatcher, new GenericMessageReceiver(configuration.MessageReceiveThreadTimeoutMilliseconds))
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
                        // TODO: Implement error handling
                    }
                    else
                    {
                        s3StorageService.DeleteObject(objectKey);
                    }
                }
                catch (Exception ex)
                {
                    // TODO: Implement error handling
                }
            }
        }

        private IMessageRequest GetBody(string objectKey)
        {
            return (IMessageRequest)SerializerJson.Deserialize(s3StorageService.DownloadObject(objectKey));
        }
    }
}
