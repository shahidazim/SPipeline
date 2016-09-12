namespace SPipeline.File
{
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Interfaces.Services;
    using SPipeline.Core.Models;
    using SPipeline.Core.Serializers;
    using SPipeline.Core.Util;
    using System;

    public class FileQueueSender : FileQueueBase, IMessageSender
    {
        public FileQueueSender(FileQueueSenderConfiguration configuration, ILoggerService loggerService, IFileSystemService fileSystemService)
            : base(configuration.BasePath, configuration.QueueName, configuration.CreateQueue, configuration.FullPath, loggerService, fileSystemService)
        {
        }

        public IMessageResponse Send<TMessageResponse>(IMessageRequest message)
            where TMessageResponse : IMessageResponse, new()
        {
            var response = new TMessageResponse();
            try
            {
                fileStorageService.Uplaod(SerializerJson.Serialize(message), ReferenceBuilder.Generate());
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
