namespace SPipeline.Cloud.AWS.SQS
{
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Models;
    using SPipeline.Core.Serializers;
    using System;

    public class AWSSQSSender : AWSSQSBase, IMessageSender
    {
        public AWSSQSSender(AWSSQSSenderConfiguration configuration, ILoggerService loggerService)
            : base(configuration.ServiceUrl, configuration.QueueName, configuration.AccountId, configuration.AccessKey, configuration.SecretKey, configuration.CreateQueue, loggerService)
        {
        }

        public IMessageResponse Send<TMessageResponse>(IMessageRequest message)
            where TMessageResponse : IMessageResponse, new()
        {
            var response = new TMessageResponse();
            try
            {
                queueService.Send(SerializerJson.Serialize(message));
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
