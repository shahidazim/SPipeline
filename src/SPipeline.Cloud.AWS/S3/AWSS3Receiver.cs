namespace SPipeline.Cloud.AWS.S3
{
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Serializers;
    using System;

    public class AWSS3Receiver : AWSS3Base, IMessageReceiver
    {
        private readonly IMessageDispatcher _messageDispatcher;

        public AWSS3Receiver(
            AWSS3ReceiverConfiguration configuration,
            IMessageDispatcher messageDispatcher,
            ILoggerService loggerService)
            : base(
                  configuration.ServiceUrl,
                  configuration.BucketName,
                  configuration.AccessKey,
                  configuration.SecretKey,
                  configuration.CreateBucket,
                  loggerService)
        {
            _messageDispatcher = messageDispatcher;
        }

        /// <summary>
        /// Receiver messages from queue and process them
        /// </summary>
        public void Process()
        {
            var references = storageService.GetAllReferences();
            foreach (var reference in references)
            {
                try
                {
                    var receivedMessage = storageService.Download(reference);
                    var message = GetBody(receivedMessage);
                    var response = _messageDispatcher.Execute(message);

                    if (response.HasError)
                    {
                        loggerService?.Error(response.GetFormattedError());
                        continue;
                    }

                    storageService.Delete(reference);
                }
                catch (Exception ex)
                {
                    loggerService?.Exception(ex);
                }
            }
        }

        private IMessageRequest GetBody(string message)
        {
            return (IMessageRequest)SerializerJson.Deserialize(message);
        }
    }
}
