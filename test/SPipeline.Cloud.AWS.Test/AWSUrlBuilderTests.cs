namespace SPipeline.Cloud.AWS.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SPipeline.Cloud.AWS.Util;
    using System;

    [TestClass]
    public class AWSUrlBuilderTests
    {
        [TestMethod]
        public void SQS_Create_ValidUrl()
        {
            Assert.AreEqual("http://aws.amazon.com/123/myqueue", AWSUrlBuilder.CreateSQSUrl("http://aws.amazon.com", "123", "myqueue").AbsoluteUri);
            Assert.AreEqual("http://aws.amazon.com/123/myqueue", AWSUrlBuilder.CreateSQSUrl("http://aws.amazon.com/", "123", "myqueue").AbsoluteUri);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SQS_Create_MissingServiceUrl_InvalidUrl()
        {
            Assert.AreEqual("http://aws.amazon.com/123/myqueue", AWSUrlBuilder.CreateSQSUrl("", "123", "myqueue").AbsoluteUri);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SQS_Create_MissingAccountId_InvalidUrl()
        {
            Assert.AreEqual("http://aws.amazon.com/123/myqueue", AWSUrlBuilder.CreateSQSUrl("http://aws.amazon.com", "", "myqueue").AbsoluteUri);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SQS_Create_MissingQueueName_InvalidUrl()
        {
            Assert.AreEqual("http://aws.amazon.com/123/myqueue", AWSUrlBuilder.CreateSQSUrl("http://aws.amazon.com/", "123", "").AbsoluteUri);
        }

        [TestMethod]
        [ExpectedException(typeof(UriFormatException))]
        public void SQS_Create_MissingQueueName_InvalidUrl1()
        {
            Assert.AreEqual("http://aws.amazon.com/123/myqueue", AWSUrlBuilder.CreateSQSUrl("aws.amazon.com", "123", "myqueue").AbsoluteUri);
        }


        [TestMethod]
        public void S3_Create_ValidUrl()
        {
            Assert.AreEqual("http://aws.amazon.com/bucket/myqueue", AWSUrlBuilder.CreateS3Url("http://aws.amazon.com", "bucket", "myqueue").AbsoluteUri);
            Assert.AreEqual("http://aws.amazon.com/bucket/myqueue", AWSUrlBuilder.CreateS3Url("http://aws.amazon.com/", "bucket", "myqueue").AbsoluteUri);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void S3_Create_MissingServiceUrl_InvalidUrl()
        {
            Assert.AreEqual("http://aws.amazon.com/123/myqueue", AWSUrlBuilder.CreateS3Url("", "bucket", "myqueue").AbsoluteUri);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void S3_Create_MissingAccountId_InvalidUrl()
        {
            Assert.AreEqual("http://aws.amazon.com/123/myqueue", AWSUrlBuilder.CreateS3Url("http://aws.amazon.com", "", "myqueue").AbsoluteUri);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void S3_Create_MissingQueueName_InvalidUrl()
        {
            Assert.AreEqual("http://aws.amazon.com/123/myqueue", AWSUrlBuilder.CreateS3Url("http://aws.amazon.com/", "bucket", "").AbsoluteUri);
        }

        [TestMethod]
        [ExpectedException(typeof(UriFormatException))]
        public void S3_Create_MissingQueueName_InvalidUrl1()
        {
            Assert.AreEqual("http://aws.amazon.com/123/myqueue", AWSUrlBuilder.CreateS3Url("aws.amazon.com", "bucket", "myqueue").AbsoluteUri);
        }

    }
}
