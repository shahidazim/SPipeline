namespace SPipeline.Cloud.AWS.SQS
{
    using Amazon.SQS.Model;
    using SPipeline.Cloud.AWS.Util;
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Serializers;
    using SPipeline.Core.Services;
    using System.Net;

    public class AWSSQSReceiver : AWSSQSBase
    {
        private readonly AWSSQSReceiverConfiguration _configuration;
        private readonly IMessageDispatcher _messageDispatcher;
        private readonly IMessageReceiver _messageReceiver;

        public AWSSQSReceiver(AWSSQSReceiverConfiguration configuration, IMessageDispatcher messageDispatcher, ILoggerService loggerService)
            : base(configuration.ServiceUrl, configuration.QueueName, configuration.AccountId, configuration.AccessKey, configuration.SecretKey, configuration.CreateQueue, loggerService)
        {
            _configuration = configuration;
            _messageDispatcher = messageDispatcher;
            _messageReceiver = new GenericMessageReceiver(_configuration.MessageReceiveThreadTimeoutMilliseconds)
            {
                StartCallback = StartCallback
            };
        }

        public void Start()
        {
            _messageReceiver.Start();
        }

        public void Stop()
        {
            _messageReceiver.Stop();
        }

        private void StartCallback()
        {
            var queueUrl = AWSQueryUrlBuilder.Create(serviceUrl, accountId, queueName);
            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = _configuration.MaxNumberOfMessages
            };
            while (true)
            {
                var receiveMessageResponse = queueClient.ReceiveMessageAsync(receiveMessageRequest).Result;

                if (receiveMessageResponse.Messages.Count == 0)
                {
                    break;
                }

                foreach (var message in receiveMessageResponse.Messages)
                {
                    var response = _messageDispatcher.Execute(GetBody(message));

                    if (response.CanContinue)
                    {
                        var deleteMessageResponse =
                            queueClient.DeleteMessage(new DeleteMessageRequest(queueUrl, message.ReceiptHandle));
                        if (deleteMessageResponse.HttpStatusCode != HttpStatusCode.OK)
                        {
                            loggerService?.Error(deleteMessageResponse.ToString());
                        }
                    }

                    if (response.HasError)
                    {
                        loggerService?.Error(response.GetFormattedError());
                    }
                }
            }
        }

        private IMessageRequest GetBody(Message message)
        {
            return (IMessageRequest)SerializerJson.Deserialize(message.Body);
        }
    }
}
