namespace SPipeline.File
{
    using SPipeline.Core.Services;

    public class FileQueueReceiverConfiguration
    {
        public string BasePath { get; set; }

        public string QueueName { get; set; }

        public bool CreateQueue { get; set; }

        public string FullPath => FileSystemService.CombinePath(BasePath, QueueName);
    }
}
