namespace SPipeline.Cloud.Azure.ServiceBusQueue
{
    using SPipeline.Cloud.Azure.Services;
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Services;
    using System;

    /// <summary>
    /// The base implementation for Azure Service Bus Queue
    /// </summary>
    public abstract class AzureServiceBusQueueBase
    {
        protected IQueueService queueService;
        protected ILoggerService loggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusQueueBase" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="createQueue">if set to <c>true</c> [create queue].</param>
        /// <param name="loggerService">The logger service.</param>
        protected AzureServiceBusQueueBase(
            string connectionString,
            string queueName,
            bool createQueue,
            ILoggerService loggerService)
        {
            Initialize(connectionString, queueName, loggerService, createQueue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusQueueBase" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="createQueue">if set to <c>true</c> [create queue].</param>
        /// <param name="messageTimeToLive">The message time to live.</param>
        /// <param name="maxSizeInMegabytes">The maximum size in megabytes.</param>
        protected AzureServiceBusQueueBase(
            string connectionString,
            string queueName,
            bool createQueue,
            TimeSpan messageTimeToLive,
            int maxSizeInMegabytes,
            ILoggerService loggerService)
        {
            Initialize(connectionString, queueName, loggerService, createQueue, messageTimeToLive, maxSizeInMegabytes);
        }

        /// <summary>
        /// Initializes the queue client.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="logService">The logger service.</param>
        /// <param name="createQueue">if set to <c>true</c> [create queue].</param>
        /// <param name="messageTimeToLive">The message time to live.</param>
        /// <param name="maxSizeInMegabytes">The maximum size in megabytes.</param>
        private void Initialize(
            string connectionString,
            string queueName,
            ILoggerService logService,
            bool createQueue = false,
            TimeSpan? messageTimeToLive = null,
            int maxSizeInMegabytes = 0)
        {
            this.loggerService = logService;
            queueService = new AzureServiceBusQueueService(connectionString, queueName, createQueue, messageTimeToLive, maxSizeInMegabytes);
        }
    }
}
