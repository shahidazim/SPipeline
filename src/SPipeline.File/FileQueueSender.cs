namespace SPipeline.File
{
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Services;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Models;
    using SPipeline.Core.Serializers;
    using System;

    public class FileQueueSender : FileQueueBase, IMessageSender
    {
        private readonly FileQueueSenderConfiguration _configuration;

        public FileQueueSender(FileQueueSenderConfiguration configuration, ILoggerService loggerService)
            : base(configuration.BasePath, configuration.QueueName, configuration.CreateQueue, loggerService)
        {
            _configuration = configuration;
        }

        public IMessageResponse Send<TMessageResponse>(IMessageRequest message)
            where TMessageResponse : IMessageResponse, new()
        {
            var response = new TMessageResponse();
            try
            {
                var filePath = FileSystemService.CombinePath(_configuration.FullPath, Guid.NewGuid().ToString());
                fileSystemService.CreateFile(SerializerJson.Serialize(message), filePath);
            }
            catch (Exception ex)
            {
                loggerService?.Exception(ex);
                response.AddError(new MessageError(ex, false));
            }

            return response;
        }
    }
}
