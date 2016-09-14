namespace SPipeline.Cloud.AWS.Services
{
    using Amazon.Runtime;
    using Amazon.S3;
    using Amazon.S3.Model;
    using Amazon.S3.Util;
    using SPipeline.Core.Interfaces.Services;
    using SPipeline.Cloud.AWS.Util;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;

    public class AWSS3Service : IStorageService
    {
        private readonly string _serviceUrl;
        private readonly string _bucketName;
        private readonly AmazonS3Client _s3Client;

        public AWSS3Service(
            string serviceUrl,
            string bucketName,
            string accessKey,
            string secretKey,
            bool createBucket)
        {
            _serviceUrl = serviceUrl;
            _bucketName = bucketName;

            var awsCredentials = new BasicAWSCredentials(accessKey, secretKey);
            var s3Config = new AmazonS3Config
            {
                ServiceURL = serviceUrl
            };
            _s3Client = new AmazonS3Client(awsCredentials, s3Config);

            if (createBucket)
            {
                CreateBucket();
            }
        }

        private void CreateBucket()
        {
            if (AmazonS3Util.DoesS3BucketExist(_s3Client, _bucketName))
            {
                return;
            }

            var putBucketRequest = new PutBucketRequest
            {
                BucketName = _bucketName,
                UseClientRegion = true
            };
            var response = _s3Client.PutBucket(putBucketRequest);

            if (AWSResponseValidator.IsValid(response))
            {
                throw new AWSS3ServiceException(response.ToString());
            }
        }

        public Uri Uplaod(string content, string reference)
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = reference,
                ContentBody = content
            };
            var response = _s3Client.PutObject(request);

            if (AWSResponseValidator.IsValid(response))
            {
                throw new AWSS3ServiceException(response.ToString());
            }

            return AWSUrlBuilder.CreateS3Url(_serviceUrl, _bucketName, reference);
        }

        public string Download(string reference)
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = reference
            };

            using (var response = _s3Client.GetObject(request))
            {
                if (AWSResponseValidator.IsValid(response))
                {
                    throw new AWSS3ServiceException(response.ToString());
                }

                using (var responseStream = response.ResponseStream)
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        public void Delete(string reference)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = reference
            };

            var response = _s3Client.DeleteObject(request);

            if (AWSResponseValidator.IsValid(response))
            {
                throw new AWSS3ServiceException(response.ToString());
            }
        }

        public IEnumerable<string> GetAllReferences()
        {
            var references = new List<string>();
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                MaxKeys = 10
            };
            ListObjectsV2Response response;
            do
            {
                response = _s3Client.ListObjectsV2(request);

                if (AWSResponseValidator.IsValid(response))
                {
                    throw new AWSS3ServiceException(response.ToString());
                }

                references.AddRange(response.S3Objects.Select(entry => entry.Key));
                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated);
            return references;
        }
    }
}
