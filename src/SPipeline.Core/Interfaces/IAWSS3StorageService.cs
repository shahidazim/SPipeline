namespace SPipeline.Core.Interfaces
{
    using System.Collections.Generic;

    public interface IAWSS3StorageService
    {
        void UplaodObject(string content, string objectKey);

        string DownloadObject(string objectKey);

        void DeleteObject(string objectKey);

        List<string> GetAllObjectKeys();
    }
}
