namespace SPipeline.File
{
    using SPipeline.Core.Interfaces.Services;
    using SPipeline.Core.Services;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class FileQueueService : IStorageService
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly string _queueFullPath;

        public FileQueueService(
            string basePath,
            string queueName,
            bool createQueue,
            string queueFullPath,
            IFileSystemService fileSystemService)
        {
            _fileSystemService = fileSystemService;
            _queueFullPath = queueFullPath;

            if (!_fileSystemService.IsDirectoryExist(basePath))
            {
                throw new DirectoryNotFoundException();
            }

            if (createQueue)
            {
                CreateQueue(basePath, queueName);
            }
        }

        public Uri Uplaod(string content, string reference)
        {
            var filePath = FileSystemService.CombinePath(_queueFullPath, reference);
            _fileSystemService.CreateFile(content, filePath);
            return new Uri(filePath);
        }

        private void CreateQueue(string basePath, string queueName)
        {
            _fileSystemService.CreateDirectory(basePath, queueName);
        }

        public string Download(string reference)
        {
            return _fileSystemService.GetFileContent(reference);
        }

        public void Delete(string reference)
        {
            _fileSystemService.DeleteFile(reference);
        }

        public IEnumerable<string> GetAllReferences()
        {
            return _fileSystemService.GetFiles(_queueFullPath, "*.*");
        }
    }
}
