namespace SPipeline.Cloud.Azure
{
    using Microsoft.ServiceBus.Messaging;
    using SPipeline.Core.Interfaces;
    using SPipeline.Core.Serializers;
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// The Azure Service Bus Receiver wrapper to receive and deserialize the messages
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="SPipeline.Cloud.Azure.AzureServiceBusBase" />
    /// <seealso cref="SPipeline.Core.Interfaces.IMessageReceiver" />
    public class AzureServiceBusReceiver : AzureServiceBusBase, IMessageReceiver
    {
        private AzureServiceBusReceiverConfiguration _configuration;
        private readonly IMessageDispatcher _messageDispatcher;
        private readonly object _lockObject = new object();
        private bool _isRunning;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusReceiver"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="messageDispatcher">The message dispatcher.</param>
        public AzureServiceBusReceiver(AzureServiceBusReceiverConfiguration configuration, IMessageDispatcher messageDispatcher)
            : base(configuration.ConnectionString, configuration.QueueName)
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

        /// <summary>
        /// Starts listening to receive the messages from Azure Service Bus queue.
        /// </summary>
        public void Start()
        {
            IsRunning = true;
            var task = Task.Factory.StartNew(() =>
            {
                while (IsRunning)
                {
                    while (true)
                    {
                        var receivedMessage = QueueClient.ReceiveAsync().Result;

                        if (receivedMessage == null)
                        {
                            break;
                        }

                        try
                        {
                            var message = GetBody(receivedMessage);
                            var response = _messageDispatcher.Execute(message);

                            if (response.HasError)
                            {
                                // TODO: Implement error handling
                            }

                            receivedMessage.Complete();
                        }
                        catch (Exception ex)
                        {
                            // TODO: Implement error handling
                            receivedMessage.Abandon();
                        }
                    }
                    Thread.Sleep(_configuration.MessageReceiveThreadTimeoutMilliseconds);
                }
            });

            task.Wait();
        }

        /// <summary>
        /// Close the Azure Service Bus Queue connection.
        /// </summary>
        public void Stop()
        {
            QueueClient.Close();
        }


        /// <summary>
        /// Deserialized the message body from brokered message.
        /// </summary>
        /// <param name="brokeredMessage">The brokered message.</param>
        /// <returns></returns>
        private IMessageRequest GetBody(BrokeredMessage brokeredMessage)
        {
            var stream = brokeredMessage.GetBody<Stream>();
            var reader = new StreamReader(stream);
            var messageBody = reader.ReadToEnd();
            return (IMessageRequest)SerializerJson.Deserialize(messageBody);
        }
    }
}
