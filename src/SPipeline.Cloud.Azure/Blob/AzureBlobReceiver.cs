namespace SPipeline.Cloud.Azure.Blob
{
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Serializers;
    using SPipeline.Core.Services;
    using System;

    public class AzureBlobReceiver : AzureBlobBase
    {
        private readonly IMessageDispatcher _messageDispatcher;
        private readonly IMessageReceiver _messageReceiver;

        public AzureBlobReceiver(AzureBlobReceiverConfiguration configuration, IMessageDispatcher messageDispatcher, IMessageReceiver messageReceiver, ILoggerService loggerService)
            : base(configuration.ConnectionString, configuration.QueueName, configuration.CreateQueue, loggerService)
        {
            _messageDispatcher = messageDispatcher;
            _messageReceiver = messageReceiver;
            _messageReceiver.StartCallback = StartCallback;
        }

        public AzureBlobReceiver(AzureBlobReceiverConfiguration configuration, IMessageDispatcher messageDispatcher, ILoggerService loggerService)
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
            var blobPaths = blobStorageService.GetAllBlockBlobs();

            foreach (var blobPath in blobPaths)
            {
                try
                {
                    var message = GetBody(blobPath);
                    var response = _messageDispatcher.Execute(message);

                    if (response.HasError)
                    {
                        loggerService?.Error(response.GetFormattedError());
                    }
                    else
                    {
                        blobStorageService.DeleteBlob(blobPath);
                    }
                }
                catch (Exception ex)
                {
                    loggerService?.Exception(ex);
                }
            }
        }

        private IMessageRequest GetBody(string blobPath)
        {
            return (IMessageRequest)SerializerJson.Deserialize(blobStorageService.DownloadContent(blobPath));
        }
    }
}
