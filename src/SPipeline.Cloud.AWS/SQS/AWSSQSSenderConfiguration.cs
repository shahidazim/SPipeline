namespace SPipeline.Cloud.AWS.SQS
{
    public class AWSSQSSenderConfiguration
    {
        public string ServiceUrl { get; set; }

        public string AccountId { get; set; }

        public string QueueName { get; set; }

        public string AccessKey { get; set; }

        public string SecretKey { get; set; }

        public bool CreateQueue { get; set; }
    }
}
