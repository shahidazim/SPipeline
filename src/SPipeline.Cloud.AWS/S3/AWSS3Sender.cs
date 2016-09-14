namespace SPipeline.Cloud.AWS.S3
{
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Models;
    using SPipeline.Core.Serializers;
    using SPipeline.Core.Util;
    using System;

    public class AWSS3Sender : AWSS3Base, IMessageSender
    {
        public AWSS3Sender(
            AWSS3SenderConfiguration configuration,
            ILoggerService loggerService)
            : base(
                  configuration.ServiceUrl,
                  configuration.BucketName,
                  configuration.AccessKey,
                  configuration.SecretKey,
                  configuration.CreateBucket,
                  loggerService)
        {
        }

        public IMessageResponse Send<TMessageResponse>(IMessageRequest message)
            where TMessageResponse : IMessageResponse, new()
        {
            var response = new TMessageResponse();
            try
            {
                storageService.Uplaod(SerializerJson.Serialize(message), ReferenceBuilder.Generate());
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
