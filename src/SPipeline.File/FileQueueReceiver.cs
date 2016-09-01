namespace SPipeline.File
{
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Serializers;
    using System;

    public class FileQueueReceiver : FileQueueBase, IMessageReceiver
    {
        private readonly FileQueueReceiverConfiguration _configuration;
        private readonly IMessageDispatcher _messageDispatcher;

        public FileQueueReceiver(FileQueueReceiverConfiguration configuration, IMessageDispatcher messageDispatcher, ILoggerService loggerService)
            : base(configuration.BasePath, configuration.QueueName, configuration.CreateQueue, loggerService)
        {
            _configuration = configuration;
            _messageDispatcher = messageDispatcher;
        }

        /// <summary>
        /// Receiver messages from queue and process them
        /// </summary>
        public void Process()
        {
            var filePaths = fileSystemService.GetFiles(_configuration.FullPath, "*.*");

            foreach (var filePath in filePaths)
            {
                try
                {
                    var message = GetBody(filePath);
                    var response = _messageDispatcher.Execute(message);

                    if (response.HasError)
                    {
                        loggerService?.Error(response.GetFormattedError());
                    }
                    else
                    {
                        fileSystemService.DeleteFile(filePath);
                    }
                }
                catch (Exception ex)
                {
                    loggerService?.Exception(ex);
                }
            }
        }

        private IMessageRequest GetBody(string filePath)
        {
            return (IMessageRequest)SerializerJson.Deserialize(fileSystemService.GetFileContent(filePath));
        }
    }
}
