namespace SPipeline.Cloud.AWS.Services
{
    using System;

    public class AWSSQSServiceException : Exception
    {
        public AWSSQSServiceException(string message) : base(message)
        {
        }
    }
}
