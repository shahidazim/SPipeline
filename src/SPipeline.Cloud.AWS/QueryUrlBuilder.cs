namespace SPipeline.Cloud.AWS
{
    using System;

    public static class QueryUrlBuilder
    {
        public static string Create(string serviceUrl, string accountId, string queueName)
        {
            if (string.IsNullOrWhiteSpace(serviceUrl))
            {
                throw new ArgumentException("serviceUrl");
            }

            if (string.IsNullOrWhiteSpace(accountId))
            {
                throw new ArgumentException("accountId");
            }

            if (string.IsNullOrWhiteSpace(queueName))
            {
                throw new ArgumentException("queueName");
            }

            if (serviceUrl.EndsWith("/"))
            {
                serviceUrl = serviceUrl.Substring(0, serviceUrl.Length - 1);
            }

            return new Uri($"{serviceUrl}/{accountId}/{queueName}").AbsoluteUri;
        }
    }
}
