namespace SPipeline.Cloud.Azure.ServiceBus
{
    using SPipeline.Cloud.Azure.Services;
    using SPipeline.Core.Interfaces.Services;
    using SPipeline.Core.DebugHelper;
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
        protected AzureServiceBusQueueBase(string connectionString, string queueName, bool createQueue, ILoggerService loggerService)
        {
            Initialize(connectionString, queueName, loggerService, null, 0, createQueue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusQueueBase" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="messageTimeToLive">The message time to live.</param>
        /// <param name="maxSizeInMegabytes">The maximum size in megabytes.</param>
        /// <param name="createQueue">if set to <c>true</c> [create queue].</param>
        protected AzureServiceBusQueueBase(string connectionString, string queueName, TimeSpan messageTimeToLive, int maxSizeInMegabytes, bool createQueue, ILoggerService loggerService)
        {
            Initialize(connectionString, queueName, loggerService, messageTimeToLive, maxSizeInMegabytes, createQueue);
        }

        /// <summary>
        /// Initializes the queue client.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="loggerService">The logger service.</param>
        /// <param name="messageTimeToLive">The message time to live.</param>
        /// <param name="maxSizeInMegabytes">The maximum size in megabytes.</param>
        /// <param name="createQueue">if set to <c>true</c> [create queue].</param>
        private void Initialize(string connectionString, string queueName, ILoggerService loggerService, TimeSpan? messageTimeToLive = null, int maxSizeInMegabytes = 0, bool createQueue = false)
        {
            this.loggerService = loggerService;
            queueService = new AzureServiceBusQueueService(connectionString, queueName, messageTimeToLive, maxSizeInMegabytes, createQueue);
        }
    }
}
