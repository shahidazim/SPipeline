namespace SPipeline.Cloud.Azure.Blob
{
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Models;
    using SPipeline.Core.Serializers;
    using System;

    public class AzureBlobSender : AzureBlobBase, IMessageSender
    {
        public AzureBlobSender(AzureBlobSenderConfiguration configuration, ILoggerService loggerService)
            : base(configuration.ConnectionString, configuration.QueueName, configuration.CreateQueue, loggerService)
        {
        }

        public IMessageResponse Send<TMessageResponse>(IMessageRequest message)
            where TMessageResponse : IMessageResponse, new()
        {
            var response = new TMessageResponse();
            try
            {
                blobStorageService.UplaodContent(SerializerJson.Serialize(message), Guid.NewGuid().ToString());
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
