namespace SPipeline.Cloud.Azure
{
    using Microsoft.ServiceBus.Messaging;
    using SPipeline.Core;
    using SPipeline.Core.Interfaces;
    using SPipeline.Core.Serializers;
    using System;
    using System.IO;

    /// <summary>
    /// The Azure Service Bus Receiver wrapper to receive and deserialize the messages
    /// </summary>
    /// <seealso cref="SPipeline.Cloud.Azure.AzureServiceBusBase" />
    /// <seealso cref="SPipeline.Core.Interfaces.IMessageReceiver" />
    public class AzureServiceBusReceiver : AzureServiceBusBase
    {
        private readonly IMessageDispatcher _messageDispatcher;
        private readonly IMessageReceiver _messageReceiver;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusReceiver"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="messageDispatcher">The message dispatcher.</param>
        /// <param name="messageReceiver">The message receiver.</param>
        public AzureServiceBusReceiver(AzureServiceBusReceiverConfiguration configuration, IMessageDispatcher messageDispatcher, IMessageReceiver messageReceiver)
            : base(configuration.ConnectionString, configuration.QueueName, configuration.CreateQueue)
        {
            _messageDispatcher = messageDispatcher;
            _messageReceiver = messageReceiver;
            _messageReceiver.StartCallback = StartCallback;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusReceiver"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="messageDispatcher">The message dispatcher.</param>
        public AzureServiceBusReceiver(AzureServiceBusReceiverConfiguration configuration, IMessageDispatcher messageDispatcher)
            : this(configuration, messageDispatcher, new GenericMessageReceiver(configuration.MessageReceiveThreadTimeoutMilliseconds))
        {
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
