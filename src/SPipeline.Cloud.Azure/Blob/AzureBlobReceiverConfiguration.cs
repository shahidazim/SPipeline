namespace SPipeline.Cloud.Azure.Blob
{
    public class AzureBlobReceiverConfiguration
    {
        public string ConnectionString { get; set; }

        public string QueueName { get; set; }

        public bool CreateQueue { get; set; }
    }
}
