namespace SPipeline.Core.Interfaces.Services
{
    using System;
    using System.Collections.Generic;

    public interface IBlobStorageService
    {
        Uri UplaodContent(string content, string blobName);

        string DownloadContent(string blobName);

        void DeleteBlob(string blobName);

        List<string> GetAllBlockBlobs();
    }
}
