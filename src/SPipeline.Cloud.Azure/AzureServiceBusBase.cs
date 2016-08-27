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

        protected QueueClient QueueClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusBase" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="createQueue">if set to <c>true</c> [create queue].</param>
        protected AzureServiceBusBase(string connectionString, string queueName, bool createQueue)
        {
            Initialize(connectionString, queueName, null, 0, createQueue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusBase"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="messageTimeToLive">The message time to live.</param>
        /// <param name="maxSizeInMegabytes">The maximum size in megabytes.</param>
        protected AzureServiceBusBase(string connectionString, string queueName, TimeSpan messageTimeToLive, int maxSizeInMegabytes, bool createQueue)
        {
            Initialize(connectionString, queueName, messageTimeToLive, maxSizeInMegabytes, createQueue);
        }

        /// <summary>
        /// Initializes the queue client.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="messageTimeToLive">The message time to live.</param>
        /// <param name="maxSizeInMegabytes">The maximum size in megabytes.</param>
        /// <param name="createQueue">if set to <c>true</c> [create queue].</param>
        private void Initialize(string connectionString, string queueName, TimeSpan? messageTimeToLive = null, int maxSizeInMegabytes = 0, bool createQueue = false)
        {
            if (createQueue)
            {
                CreateQueue(connectionString, queueName, messageTimeToLive, maxSizeInMegabytes);
            }
            QueueClient = QueueClient.CreateFromConnectionString(connectionString, queueName);
        }

        /// <summary>
        /// Creates the queue if not already exist.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="messageTimeToLive">The message time to live.</param>
        /// <param name="maxSizeInMegabytes">The maximum size in megabytes.</param>
        private void CreateQueue(string connectionString, string queueName, TimeSpan? messageTimeToLive, int maxSizeInMegabytes)
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            // Queue is already exists, just return
            if (namespaceManager.QueueExists(queueName))
            {
                return;
            }

            var queueDescription = new QueueDescription(queueName)
            {
                MaxSizeInMegabytes = maxSizeInMegabytes == 0 ? DefaultMaxSizeInMegabytes : maxSizeInMegabytes,
            };
            if (messageTimeToLive.HasValue)
            {
                queueDescription.DefaultMessageTimeToLive = messageTimeToLive.Value;
            }
            namespaceManager.CreateQueue(queueDescription);
        }
    }
}
