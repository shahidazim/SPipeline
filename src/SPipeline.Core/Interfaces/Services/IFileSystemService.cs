namespace SPipeline.Core.Interfaces.Services
{
    public interface IFileSystemService
    {
        void DeleteFile(string pathToFile);

        string[] GetFiles(string path, string pattern);

        void CreateFile(string document, string pathToFile);

        void CreateDirectory(string basePath, string folderName);

        bool IsDirectoryExist(string path);

        string GetFileContent(string filePath);
    }
}
