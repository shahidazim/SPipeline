namespace SPipeline.File
{
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Interfaces.Services;
    using SPipeline.Core.Serializers;
    using System;

    public class FileQueueReceiver : FileQueueBase, IMessageReceiver
    {
        private readonly IMessageDispatcher _messageDispatcher;

        public FileQueueReceiver(FileQueueReceiverConfiguration configuration, IMessageDispatcher messageDispatcher, ILoggerService loggerService, IFileSystemService fileSystemService)
            : base(configuration.BasePath, configuration.QueueName, configuration.CreateQueue, configuration.FullPath, loggerService, fileSystemService)
        {
            _messageDispatcher = messageDispatcher;
        }

        /// <summary>
        /// Receiver messages from queue and process them
        /// </summary>
        public void Process()
        {
            var filePaths = fileStorageService.GetAllReferences();

            foreach (var filePath in filePaths)
            {
                try
                {
                    var receivedMessage = fileStorageService.Download(filePath);
                    var message = GetBody(receivedMessage);
                    var response = _messageDispatcher.Execute(message);

                    if (response.HasError)
                    {
                        loggerService?.Error(response.GetFormattedError());
                        continue;
                    }

                    fileStorageService.Delete(filePath);
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
