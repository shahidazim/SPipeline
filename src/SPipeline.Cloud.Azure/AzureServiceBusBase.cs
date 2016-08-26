namespace SPipeline.Cloud.Azure
{
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using System;

    /// <summary>
    /// The base implementation for Azure Service Bus wapper
    /// </summary>
    public abstract class AzureServiceBusBase
    {
        private const int DefaultMaxSizeInMegabytes = 1024;
        private readonly TimeSpan _messageTimeToLive;
        private readonly int _maxSizeInMegabytes;

        protected QueueClient QueueClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusBase"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="queueName">Name of the queue.</param>
        protected AzureServiceBusBase(string connectionString, string queueName)
        {
            Initialize(connectionString, queueName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusBase"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="messageTimeToLive">The message time to live.</param>
        /// <param name="maxSizeInMegabytes">The maximum size in megabytes.</param>
        protected AzureServiceBusBase(string connectionString, string queueName, TimeSpan messageTimeToLive, int maxSizeInMegabytes)
        {
            _messageTimeToLive = messageTimeToLive;
            _maxSizeInMegabytes = maxSizeInMegabytes;
            Initialize(connectionString, queueName);
        }

        /// <summary>
        /// Initializes the queue client.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="queueName">Name of the queue.</param>
        private void Initialize(string connectionString, string queueName)
        {
            CreateQueue(connectionString, queueName);
            QueueClient = QueueClient.CreateFromConnectionString(connectionString, queueName);
        }

        /// <summary>
        /// Creates the queue if not already exist.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="queueName">Name of the queue.</param>
        private void CreateQueue(string connectionString, string queueName)
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            if (namespaceManager.QueueExists(queueName))
            {
                return;
            }

            var queueDescription = new QueueDescription(queueName)
            {
                MaxSizeInMegabytes = _maxSizeInMegabytes == 0 ? DefaultMaxSizeInMegabytes : _maxSizeInMegabytes,
                DefaultMessageTimeToLive = _messageTimeToLive
            };
            namespaceManager.CreateQueue(queueDescription);
        }
    }
}
