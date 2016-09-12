namespace SPipeline.Cloud.AWS.Util
{
    using Amazon.Runtime;
    using System.Net;

    public static class AWSResponseValidator
    {
        public static bool IsValid(AmazonWebServiceResponse response)
        {
            return response.HttpStatusCode != HttpStatusCode.OK && response.HttpStatusCode != HttpStatusCode.NoContent;
        }
    }
}
