using SPipeline.Core.Services;

namespace SPipeline.Cloud.AWS
{
    using Amazon.SQS.Model;
    using SPipeline.Core;
    using SPipeline.Core.Interfaces;
    using SPipeline.Core.Serializers;
    using System.Net;

    public class SimpleQueueServiceReceiver : SimpleQueueServiceBase
    {
        private readonly SimpleQueueServiceReceiverConfiguration _configuration;
        private readonly IMessageDispatcher _messageDispatcher;
        private readonly IMessageReceiver _messageReceiver;

        public SimpleQueueServiceReceiver(SimpleQueueServiceReceiverConfiguration configuration, IMessageDispatcher messageDispatcher)
            : base(configuration.ServiceUrl, configuration.QueueName, configuration.AccountId, configuration.AccessKey, configuration.SecretKey, configuration.CreateQueue)
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
            var queueUrl = QueryUrlBuilder.Create(serviceUrl, accountId, queueName);
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
                    var response = _messageDispatcher.Execute((IMessageRequest)SerializerJson.Deserialize(message.Body));

                    if (response.CanContinue)
                    {
                        var deleteMessageResponse =
                            queueClient.DeleteMessage(new DeleteMessageRequest(queueUrl, message.ReceiptHandle));
                        if (deleteMessageResponse.HttpStatusCode != HttpStatusCode.OK)
                        {
                            // TODO: Implement error handling
                        }
                    }

                    if (response.HasError)
                    {
                        // TODO: Implement error handling
                    }
                }
            }
        }
    }
}
