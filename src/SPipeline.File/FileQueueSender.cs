namespace SPipeline.File
{
    using SPipeline.Core.Services;
    using SPipeline.Core.Interfaces;
    using SPipeline.Core.Models;
    using SPipeline.Core.Serializers;
    using System;

    public class FileQueueSender : FileQueueBase, IMessageSender
    {
        private readonly FileQueueSenderConfiguration _configuration;

        public FileQueueSender(FileQueueSenderConfiguration configuration)
            : base(configuration.BasePath, configuration.QueueName, configuration.CreateQueue)
        {
            _configuration = configuration;
        }

        public IMessageResponse Send<TMessageResponse>(IMessageRequest message)
            where TMessageResponse : IMessageResponse, new()
        {
            return Send<TMessageResponse>(SerializerJson.Serialize(message));
        }

        private IMessageResponse Send<TMessageResponse>(string payload)
            where TMessageResponse : IMessageResponse, new()
        {
            var response = new TMessageResponse();
            try
            {
                var filePath = FileSystemService.CombinePath(_configuration.FullPath, Guid.NewGuid().ToString());
                fileSystemService.CreateFile(payload, filePath);
            }
            catch (Exception ex)
            {
                response.AddError(new MessageError(ex, false));
            }

            return response;

        }
    }
}
