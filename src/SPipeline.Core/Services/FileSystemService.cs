using SPipeline.Core.Interfaces.Services;

namespace SPipeline.Core.Services
{
    using SPipeline.Core.Interfaces;
    using System.IO;

    public class FileSystemService : IFileSystemService
    {
        public void MoveFile(string sourceFile, string destinationFile)
        {
            File.Move(sourceFile, destinationFile);
        }

        public void DeleteFile(string pathToFile)
        {
            File.Delete(pathToFile);
        }

        public string[] GetFiles(string path, string pattern)
        {
            return Directory.GetFiles(path, pattern);
        }

        public void CreateFile(string document, string pathToFile)
        {
            File.WriteAllText(pathToFile, document);
        }

        public void CreateDirectory(string basePath, string folderName)
        {
            Directory.CreateDirectory(CombinePath(basePath, folderName));
        }

        public bool IsDirectoryExist(string path)
        {
            return Directory.Exists(path);
        }

        public string GetFileContent(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public static string CombinePath(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }
    }
}
