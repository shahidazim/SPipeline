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

            const string serviceUrl = "https://sqs.<region>.amazonaws.com/";
            const string accountId = "<account-id>";
            const string queueName = "<queue-name>";
            const string accessKey = "<access-key>";
            const string secretKey = "<secret-key>";

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

            try
            {
                var sender = new AWSSQSSender(simpleQueueServiceSenderConfiguration, loggerService);
                sender.Send<MyMessageResponse>(message);
                sender.Send<MyMessageResponse>(message);

                var messageDispatcher = new MessageDispatcher().RegisterPipeline(genericPipeline);
                var receiver = new AWSSQSReceiver(simpleQueueServiceReceiveConfiguration, messageDispatcher, loggerService);
                receiver.Process();
            }
            catch (Exception ex)
            {
                loggerService.Exception(ex);
            }
        }
    }
}
