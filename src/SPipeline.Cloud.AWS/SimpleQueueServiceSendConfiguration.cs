namespace SPipeline.Cloud.AWS
{
    public class SimpleQueueServiceSendConfiguration
    {
        public string ServiceUrl { get; set; }

        public string AccountId { get; set; }

        public string QueueName { get; set; }

        public string AccessKey { get; set; }

        public string SecretKey { get; set; }
    }
}
