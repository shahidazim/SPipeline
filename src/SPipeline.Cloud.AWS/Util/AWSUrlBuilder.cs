namespace SPipeline.Cloud.AWS.Util
{
    using System;

    public static class AWSUrlBuilder
    {
        public static Uri CreateSQSUrl(string serviceUrl, string accountId, string queueName)
        {
            if (string.IsNullOrWhiteSpace(serviceUrl))
            {
                throw new ArgumentException(nameof(serviceUrl));
            }

            if (string.IsNullOrWhiteSpace(accountId))
            {
                throw new ArgumentException(nameof(accountId));
            }

            if (string.IsNullOrWhiteSpace(queueName))
            {
                throw new ArgumentException(nameof(queueName));
            }

            if (serviceUrl.EndsWith("/"))
            {
                serviceUrl = serviceUrl.Substring(0, serviceUrl.Length - 1);
            }

            return new Uri($"{serviceUrl}/{accountId}/{queueName}");
        }

        public static Uri CreateS3Url(string serviceUrl, string bucketName, string reference)
        {
            if (string.IsNullOrWhiteSpace(serviceUrl))
            {
                throw new ArgumentException(nameof(serviceUrl));
            }

            if (string.IsNullOrWhiteSpace(bucketName))
            {
                throw new ArgumentException(nameof(bucketName));
            }

            if (string.IsNullOrWhiteSpace(reference))
            {
                throw new ArgumentException(nameof(reference));
            }

            if (serviceUrl.EndsWith("/"))
            {
                serviceUrl = serviceUrl.Substring(0, serviceUrl.Length - 1);
            }

            return new Uri($"{serviceUrl}/{bucketName}/{reference}");
        }
    }
}
