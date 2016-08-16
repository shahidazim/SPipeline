namespace SPipeline.Cloud.AWS
{
    public class SimpleQueueServiceSenderConfiguration
    {
        public string ServiceUrl { get; set; }

        public string AccountId { get; set; }

        public string QueueName { get; set; }

        public string AccessKey { get; set; }

        public string SecretKey { get; set; }
    }
}
