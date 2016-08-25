namespace SPipeline.Cloud.AWS
{
    using Amazon.SQS.Model;
    using SPipeline.Core.Interfaces;
    using SPipeline.Core.Serializers;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class SimpleQueueServiceReceiver<T> : SimpleQueueServiceBase, IMessageReceiver
    {
        private readonly SimpleQueueServiceConfiguration _configuration;
        private readonly IMessageDispatcher _messageDispatcher;
        private readonly object _lockObject = new object();
        private bool _isRunning;

        public SimpleQueueServiceReceiver(SimpleQueueServiceConfiguration configuration, IMessageDispatcher messageDispatcher)
            : base(configuration.ServiceUrl, configuration.QueueName, configuration.AccountId, configuration.AccessKey, configuration.SecretKey)
        {
            _configuration = configuration;
            _messageDispatcher = messageDispatcher;
        }

        public bool IsRunning
        {
            get
            {
                lock (_lockObject)
                {
                    return _isRunning;
                }
            }

            set
            {
                lock (_lockObject)
                {
                    _isRunning = value;
                }
            }
        }

        public void Start()
        {
            IsRunning = true;
            var task = Task.Factory.StartNew(() =>
            {
                while (IsRunning)
                {
                    var queueUrl = QueryUrlBuilder.Create(ServiceUrl, AccountId, QueueName);
                    var receiveMessageRequest = new ReceiveMessageRequest
                    {
                        QueueUrl = queueUrl,
                        MaxNumberOfMessages = _configuration.MaxNumberOfMessages
                    };
                    var receiveMessageResponse = QueueClient.ReceiveMessage(receiveMessageRequest);
                    foreach (var message in receiveMessageResponse.Messages)
                    {
                        var response = _messageDispatcher.Execute((IMessageRequest)SerializerJson.Deserialize(message.Body));

                        if (response.CanContinue)
                        {
                            var deleteMessageResponse = QueueClient.DeleteMessage(new DeleteMessageRequest(queueUrl, message.ReceiptHandle));
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
                    Thread.Sleep(_configuration.MessageReceiveThreadTimeoutMilliseconds);
                }
            });

            task.Wait();
        }

        public void Stop()
        {
            IsRunning = false;
        }
    }
}
