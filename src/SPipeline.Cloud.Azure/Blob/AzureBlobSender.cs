namespace SPipeline.Cloud.Azure.Blob
{
    using SPipeline.Core.Interfaces;
    using SPipeline.Core.Models;
    using SPipeline.Core.Serializers;
    using System;

    public class AzureBlobSender : AzureBlobBase, IMessageSender
    {
        public AzureBlobSender(AzureBlobSenderConfiguration configuration)
            : base(configuration.ConnectionString, configuration.QueueName, configuration.CreateQueue)
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
                response.AddError(new MessageError(ex, false));
            }

            return response;
        }
    }
}
