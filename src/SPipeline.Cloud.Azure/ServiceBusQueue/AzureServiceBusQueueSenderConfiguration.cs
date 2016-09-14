namespace SPipeline.Cloud.Azure.ServiceBusQueue
{
    using System;

    /// <summary>
    /// The Azure Service Bus Queue Sender Configuration
    /// </summary>
    public class AzureServiceBusQueueSenderConfiguration
    {
        /// <summary>
        /// Gets or sets the Azure Service Bus connection string.
        /// </summary>
        /// <value>
        /// The Azure Service Bus connection string.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        public string QueueName { get; set; }

        /// <summary>
        /// Gets or sets the message time to live.
        /// </summary>
        /// <value>
        /// The message time to live.
        /// </value>
        public TimeSpan MessageTimeToLive { get; set; }

        /// <summary>
        /// Gets or sets the maximum size in megabytes.
        /// </summary>
        /// <value>
        /// The maximum size in megabytes.
        /// </value>
        public int MaxSizeInMegabytes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [create queue].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [create queue]; otherwise, <c>false</c>.
        /// </value>
        public bool CreateQueue { get; set; }
    }
}
