namespace SPipeline.File
{
    using SPipeline.Core.Interfaces.Services;
    using SPipeline.Core.Services;
    using System.IO;

    public class FileQueueBase
    {
        protected readonly IFileSystemService fileSystemService;

        public FileQueueBase(string basePath, string queueName, bool createQueue)
        {
            fileSystemService = new FileSystemService();

            if (!fileSystemService.IsDirectoryExist(basePath))
            {
                throw new DirectoryNotFoundException();
            }

            if (createQueue)
            {
                CreateQueue(basePath, queueName);
            }
        }

        private void CreateQueue(string basePath, string queueName)
        {
            fileSystemService.CreateDirectory(basePath, queueName);
        }
    }
}
