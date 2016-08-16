namespace SPipeline.Cloud.AWS
{
    public class SimpleQueueServiceSender : SimpleQueueServiceBase
    {
        public SimpleQueueServiceSender(SimpleQueueServiceSenderConfiguration configuration)
        : base(configuration.ServiceUrl, configuration.QueueName, configuration.AccountId, configuration.AccessKey, configuration.SecretKey)
        {
        }
    }
}
