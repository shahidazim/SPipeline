namespace SPipeline.Cloud.AWS.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SPipeline.Cloud.AWS.SQS;
    using SPipeline.Core.Models;
    using SPipeline.Logger.NLog;
    using SPipeline.Pipeline;
    using System;

    public class MyMessageRequest : MessageRequestBase
    {
        public MyMessageRequest(PipelineConfiguration configuration) : base(configuration)
        {
        }

        public string Name { get; set; }
    }

    public class MyMessageResponse : MessageResponseBase
    {
    }

    public class MyConfiguration
    {
    }

    [TestClass]
    public class AWSSQSTests
    {
        [TestMethod]
        [Ignore]
        [TestCategory("Integration"), TestCategory("SQS")]
        public void AWSSQS_SendAndReceiveMessages()
        {
            var message = new MyMessageRequest(new PipelineConfiguration
            {
                ClearErrorsBeforeNextHandler = false,
                BatchSizeForParallelHandlers = 10
            })
            {
                Name = "Hello World!"
            };

            var genericPipeline = new GenericPipeline<MyMessageRequest, MyMessageResponse>(new LoggerService("AWSSQS"));

            var serviceUrl = "https://sqs.<region>.amazonaws.com/";
            var accountId = "<account-id>";
            var queueName = "<queue-name>";
            var accessKey = "<access-key>";
            var secretKey = "<secret-key>";


            var simpleQueueServiceSenderConfiguration
                = new AWSSQSSenderConfiguration
                {
                    ServiceUrl = serviceUrl,
                    AccountId = accountId,
                    QueueName = queueName,
                    AccessKey = accessKey,
                    SecretKey = secretKey,
                    CreateQueue = true
                };

            var simpleQueueServiceReceiveConfiguration
                = new AWSSQSReceiverConfiguration
                {
                    ServiceUrl = serviceUrl,
                    AccountId = accountId,
                    QueueName = queueName,
                    AccessKey = accessKey,
                    SecretKey = secretKey,
                    MaxNumberOfMessages = 10,
                    CreateQueue = false
                };

            var loggerService = new LoggerService("AWS");
            try
            {
                var sender = new AWSSQSSender(simpleQueueServiceSenderConfiguration, new LoggerService("AWSSQSSender"));
                sender.Send<MyMessageResponse>(message);
                sender.Send<MyMessageResponse>(message);

                var messageDispatcher = new MessageDispatcher().RegisterPipeline(genericPipeline);
                var receiver = new AWSSQSReceiver(simpleQueueServiceReceiveConfiguration, messageDispatcher, new LoggerService("AWSSQSSender"));
                receiver.Process();
            }
            catch (Exception ex)
            {
                loggerService.Exception(ex);
            }
        }
    }
}
