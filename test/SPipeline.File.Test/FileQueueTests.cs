namespace SPipeline.File.Test
{
    using SPipeline.Core.Models;
    using SPipeline.Logger.NLog;
    using SPipeline.Pipeline;
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    public class FileQueueTests
    {
        [TestMethod]
        [Ignore]
        [TestCategory("Integration"), TestCategory("FileQueue")]
        public void FileQueue_SendAndReceiveMessages()
        {
            var message = new MyMessageRequest(new PipelineConfiguration
            {
                ClearErrorsBeforeNextHandler = false,
                BatchSizeForParallelHandlers = 10
            })
            {
                Name = "Hello World!"
            };

            var genericPipeline = new GenericPipeline<MyMessageRequest, MyMessageResponse>();

            var queueName = "<queue-name>";
            var basePath = @"<pre-exist-folder-path>";

            var fileQueueSenderConfiguration
                = new FileQueueSenderConfiguration
                {
                    BasePath = basePath,
                    QueueName = queueName,
                    CreateQueue = true
                };

            var fileQueueReceiveConfiguration
                = new FileQueueReceiverConfiguration
                {
                    BasePath = basePath,
                    QueueName = queueName,
                    MessageReceiveThreadTimeoutMilliseconds = 1000,
                    CreateQueue = false
                };

            var loggerService = new LoggerService("File");
            try
            {
                var sender = new FileQueueSender(fileQueueSenderConfiguration, loggerService);
                sender.Send<MyMessageResponse>(message);
                sender.Send<MyMessageResponse>(message);

                var messageDispatcher = new MessageDispatcher().RegisterPipeline(genericPipeline);
                var receiver = new FileQueueReceiver(fileQueueReceiveConfiguration, messageDispatcher, loggerService);
                receiver.Start();
            }
            catch (Exception ex)
            {
                loggerService.Exception(ex);
            }
        }
    }
}
