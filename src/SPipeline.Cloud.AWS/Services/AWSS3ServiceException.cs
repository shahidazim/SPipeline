namespace SPipeline.Cloud.AWS.Services
{
    using System;

    public class AWSS3ServiceException : Exception
    {
        public AWSS3ServiceException(string message) : base(message)
        {
        }
    }
}
