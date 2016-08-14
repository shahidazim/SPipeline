namespace SPipeline.Cloud.Azure
{
    using System;

    /// <summary>
    /// The Azure Service Bus Sender Configuration
    /// </summary>
    public class AzureServiceBusSenderConfiguration
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
    }
}
