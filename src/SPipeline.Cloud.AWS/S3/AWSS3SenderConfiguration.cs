namespace SPipeline.Cloud.AWS.S3
{
    public class AWSS3SenderConfiguration
    {
        public string ServiceUrl { get; set; }

        public string BucketName { get; set; }

        public string AccessKey { get; set; }

        public string SecretKey { get; set; }

        public bool CreateBucket { get; set; }
    }
}
