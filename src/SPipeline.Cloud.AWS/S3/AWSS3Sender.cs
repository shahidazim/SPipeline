namespace SPipeline.Cloud.AWS.S3
{
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Models;
    using SPipeline.Core.Serializers;
    using System;

    public class AWSS3Sender : AWSS3Base, IMessageSender
    {
        public AWSS3Sender(AWSS3SenderConfiguration configuration)
            : base(configuration.ServiceUrl, configuration.BucketName, configuration.AccessKey, configuration.SecretKey, configuration.CreateBucket)
        {
        }

        public IMessageResponse Send<TMessageResponse>(IMessageRequest message)
            where TMessageResponse : IMessageResponse, new()
        {
            var response = new TMessageResponse();
            try
            {
                s3StorageService.UplaodObject(SerializerJson.Serialize(message), Guid.NewGuid().ToString());
            }
            catch (Exception ex)
            {
                response.AddError(new MessageError(ex, false));
            }

            return response;
        }
    }
}
