namespace SPipeline.File
{
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Serializers;
    using SPipeline.Core.Services;
    using System;

    public class FileQueueReceiver : FileQueueBase
    {
        private readonly FileQueueReceiverConfiguration _configuration;
        private readonly IMessageDispatcher _messageDispatcher;
        private readonly IMessageReceiver _messageReceiver;

        public FileQueueReceiver(FileQueueReceiverConfiguration configuration, IMessageDispatcher messageDispatcher, IMessageReceiver messageReceiver, ILoggerService loggerService)
            : base(configuration.BasePath, configuration.QueueName, configuration.CreateQueue, loggerService)
        {
            _configuration = configuration;
            _messageDispatcher = messageDispatcher;
            _messageReceiver = messageReceiver;
            _messageReceiver.StartCallback = StartCallback;
        }

        public FileQueueReceiver(FileQueueReceiverConfiguration configuration, IMessageDispatcher messageDispatcher, ILoggerService loggerService)
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

        private void StartCallback()
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
