namespace SPipeline.Cloud.Azure.StorageBlob
{
    public class AzureStorageBlobReceiverConfiguration
    {
        public string ConnectionString { get; set; }

        public string QueueName { get; set; }

        public bool CreateQueue { get; set; }
    }
}
