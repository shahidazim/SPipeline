namespace SPipeline.Cloud.Azure.Services
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using SPipeline.Core.Interfaces;
    using System;
    using System.Collections.Generic;

    public class BlobStorageService : IBlobStorageService
    {
        private CloudBlobContainer _blobContainer;

        public BlobStorageService(string connectionString, string containerName, bool createQueue)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            if (createQueue)
            {
                CreateBlobContainer(blobClient, containerName);
            }
            _blobContainer = blobClient.GetContainerReference(containerName);
        }

        private void CreateBlobContainer(CloudBlobClient blobClient, string containerName)
        {
            if (_blobContainer != null)
            {
                return;
            }

            _blobContainer = blobClient.GetContainerReference(containerName);
            _blobContainer.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
        }

        public Uri UplaodContent(string content, string blobName)
        {
            var blockBlob = _blobContainer.GetBlockBlobReference(blobName);
            blockBlob.UploadText(content);
            return blockBlob.Uri;
        }

        public string DownloadContent(string blobName)
        {
            var blockBlob = _blobContainer.GetBlockBlobReference(blobName);
            return blockBlob.DownloadText();
        }

        public void DeleteBlob(string blobName)
        {
            var blockBlob = _blobContainer.GetBlockBlobReference(blobName);
            blockBlob.DeleteIfExists();
        }

        public List<string> GetAllBlockBlobs()
        {
            var blobs = new List<string>();
            foreach (var item in _blobContainer.ListBlobs())
            {
                if (item.GetType() != typeof(CloudBlockBlob))
                {
                    continue;
                }

                var blob = (CloudBlockBlob)item;
                blobs.Add(blob.Name);
            }
            return blobs;
        }
    }
}
