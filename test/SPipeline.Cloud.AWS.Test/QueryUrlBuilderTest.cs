namespace SPipeline.Cloud.AWS.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class QueryUrlBuilderTest
    {
        [TestMethod]
        public void Create_ValidUrl()
        {
            Assert.AreEqual("http://aws.amazon.com/123/myqueue", QueryUrlBuilder.Create("http://aws.amazon.com", "123", "myqueue"));
            Assert.AreEqual("http://aws.amazon.com/123/myqueue", QueryUrlBuilder.Create("http://aws.amazon.com/", "123", "myqueue"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_MissingServiceUrl_InvalidUrl()
        {
            Assert.AreEqual("http://aws.amazon.com/123/myqueue", QueryUrlBuilder.Create("", "123", "myqueue"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_MissingAccountId_InvalidUrl()
        {
            Assert.AreEqual("http://aws.amazon.com/123/myqueue", QueryUrlBuilder.Create("http://aws.amazon.com", "", "myqueue"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_MissingQueueName_InvalidUrl()
        {
            Assert.AreEqual("http://aws.amazon.com/123/myqueue", QueryUrlBuilder.Create("http://aws.amazon.com/", "123", ""));
        }

        [TestMethod]
        [ExpectedException(typeof(UriFormatException))]
        public void Create_MissingQueueName_InvalidUrl1()
        {
            Assert.AreEqual("http://aws.amazon.com/123/myqueue", QueryUrlBuilder.Create("aws.amazon.com", "123", "myqueue"));
        }
    }
}
