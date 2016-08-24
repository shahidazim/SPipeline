namespace SPipeline.Cloud.Azure
{
    using SPipeline.Core.Interfaces;
    using System;

    /// <summary>
    /// The Azure Service Bus Receiver wrapper to receive and deserialize the messages
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="SPipeline.Cloud.Azure.AzureServiceBusBase" />
    /// <seealso cref="SPipeline.Core.Interfaces.IMessageReceiver" />
    public class AzureServiceBusReceiver<T> : AzureServiceBusBase, IMessageReceiver
    {
        private readonly IMessageDispatcher _messageDispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusReceiver{T}"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="messageDispatcher">The message dispatcher.</param>
        public AzureServiceBusReceiver(AzureServiceBusReceiverConfiguration configuration, IMessageDispatcher messageDispatcher)
        : base(configuration.ConnectionString, configuration.QueueName)
        {
            _messageDispatcher = messageDispatcher;
        }

        /// <summary>
        /// Starts listening to receive the messages from Azure Service Bus queue.
        /// </summary>
        public void Start()
        {
            QueueClient.OnMessage((receivedMessage) =>
            {
                try
                {
                    var message = GetBody<IMessageRequest>(receivedMessage);

                    if (message == null)
                    {
                        return;
                    }

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
            });
        }

        /// <summary>
        /// Close the Azure Service Bus Queue connection.
        /// </summary>
        public void Stop()
        {
            QueueClient.Close();
        }
    }
}
