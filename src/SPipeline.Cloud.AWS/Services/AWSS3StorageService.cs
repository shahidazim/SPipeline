namespace SPipeline.Cloud.AWS.Services
{
    using Amazon.Runtime;
    using Amazon.S3;
    using Amazon.S3.Model;
    using Amazon.S3.Util;
    using SPipeline.Core.Interfaces.Services;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;

    public class AWSS3StorageService : IAWSS3StorageService
    {
        private readonly string _bucketName;
        private readonly AmazonS3Client _s3Client;

        public AWSS3StorageService(
            string serviceUrl,
            string bucketName,
            string accessKey,
            string secretKey,
            bool createBucket)
        {
            _bucketName = bucketName;

            var awsCredentials = new BasicAWSCredentials(accessKey, secretKey);
            var sqsConfig = new AmazonS3Config
            {
                ServiceURL = serviceUrl
            };
            _s3Client = new AmazonS3Client(awsCredentials, sqsConfig);

            if (createBucket)
            {
                CreateBucket();
            }
        }

        private void CreateBucket()
        {
            if (!AmazonS3Util.DoesS3BucketExist(_s3Client, _bucketName))
            {
                var putBucketRequest = new PutBucketRequest
                {
                    BucketName = _bucketName,
                    UseClientRegion = true
                };

                var response = _s3Client.PutBucket(putBucketRequest);
            }
        }

        public void UplaodObject(string content, string objectKey)
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = objectKey,
                ContentBody = content
            };
            var response = _s3Client.PutObject(request);
        }

        public string DownloadObject(string objectKey)
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = objectKey
            };

            using (var response = _s3Client.GetObject(request))
            {
                using (var responseStream = response.ResponseStream)
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        public void DeleteObject(string objectKey)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = objectKey
            };

            var response = _s3Client.DeleteObject(request);
        }

        public List<string> GetAllObjectKeys()
        {
            var keys = new List<string>();
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                MaxKeys = 10
            };
            ListObjectsV2Response response;
            do
            {
                response = _s3Client.ListObjectsV2(request);
                keys.AddRange(response.S3Objects.Select(entry => entry.Key));
                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated);
            return keys;
        }
    }
}
