namespace SPipeline.File
{
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Services;

    public class FileQueueBase
    {
        protected readonly IStorageService fileStorageService;
        protected readonly ILoggerService loggerService;

        public FileQueueBase(
            string basePath,
            string queueName,
            bool createQueue,
            string queueFullPath,
            ILoggerService loggerService,
            IFileSystemService fileSystemService)
        {
            this.loggerService = loggerService;
            fileStorageService = new FileQueueService(basePath, queueName, createQueue, queueFullPath, fileSystemService);
        }
    }
}
