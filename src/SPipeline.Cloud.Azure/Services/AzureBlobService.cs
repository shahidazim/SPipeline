namespace SPipeline.Cloud.Azure.Services
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using SPipeline.Core.Interfaces.Services;
    using System;
    using System.Collections.Generic;

    public class AzureBlobService : IStorageService
    {
        private CloudBlobContainer _blobContainer;

        public AzureBlobService(string connectionString, string containerName, bool createQueue)
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

        public Uri Uplaod(string content, string reference)
        {
            var blockBlob = _blobContainer.GetBlockBlobReference(reference);
            blockBlob.UploadText(content);
            return blockBlob.Uri;
        }

        public string Download(string reference)
        {
            var blockBlob = _blobContainer.GetBlockBlobReference(reference);
            return blockBlob.DownloadText();
        }

        public void Delete(string reference)
        {
            var blockBlob = _blobContainer.GetBlockBlobReference(reference);
            blockBlob.DeleteIfExists();
        }

        public IEnumerable<string> GetAllReferences()
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
