namespace SPipeline.Cloud.AWS.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SPipeline.Cloud.AWS.S3;
    using SPipeline.Core.Models;
    using SPipeline.Logger.NLog;
    using SPipeline.Pipeline;
    using System;

    [TestClass]
    public class AWSS3Tests
    {
        [TestMethod]
        [Ignore]
        [TestCategory("Integration"), TestCategory("S3")]
        public void AWSS3_SendAndReceiveMessages()
        {
            var loggerService = new LoggerService("AWS");

            var message = new MyMessageRequest(new PipelineConfiguration
            {
                ClearErrorsBeforeNextHandler = false,
                BatchSizeForParallelHandlers = 10
            })
            {
                Name = "Hello World!"
            };

            var genericPipeline = new GenericPipeline<MyMessageRequest, MyMessageResponse>(loggerService);

            const string serviceUrl = "https://s3-<region>.amazonaws.com/";
            const string bucketName = "<bucket-name>";
            const string accessKey = "<access-key>";
            const string secretKey = "<secret-key>";

            var s3SenderConfiguration
                = new AWSS3SenderConfiguration
                {
                    ServiceUrl = serviceUrl,
                    BucketName = bucketName,
                    AccessKey = accessKey,
                    SecretKey = secretKey,
                    CreateBucket = true
                };

            var s3ReceiveConfiguration
                = new AWSS3ReceiverConfiguration
                {
                    ServiceUrl = serviceUrl,
                    BucketName = bucketName,
                    AccessKey = accessKey,
                    SecretKey = secretKey,
                    CreateBucket = false
                };

            try
            {
                var sender = new AWSS3Sender(s3SenderConfiguration, loggerService);
                sender.Send<MyMessageResponse>(message);
                sender.Send<MyMessageResponse>(message);

                var messageDispatcher = new MessageDispatcher().RegisterPipeline(genericPipeline);
                var receiver = new AWSS3Receiver(s3ReceiveConfiguration, messageDispatcher, loggerService);
                receiver.Process();
            }
            catch (Exception ex)
            {
                loggerService.Exception(ex);
            }
        }
    }
}
