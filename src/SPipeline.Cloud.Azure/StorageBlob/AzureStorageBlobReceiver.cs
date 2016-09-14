namespace SPipeline.Cloud.Azure.StorageBlob
{
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Serializers;
    using System;

    public class AzureStorageBlobReceiver : AzureStorageBlobBase, IMessageReceiver
    {
        private readonly IMessageDispatcher _messageDispatcher;

        public AzureStorageBlobReceiver(
            AzureStorageBlobReceiverConfiguration configuration,
            IMessageDispatcher messageDispatcher,
            ILoggerService loggerService)
            : base(
                  configuration.ConnectionString,
                  configuration.QueueName,
                  configuration.CreateQueue,
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
