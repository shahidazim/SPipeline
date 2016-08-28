namespace SPipeline.Cloud.Azure.ServiceBus
{
    /// <summary>
    /// The Azure Service Bus Receiver Configuration
    /// </summary>
    public class AzureServiceBusReceiverConfiguration
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
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
        /// Gets or sets the message receive thread timeout milliseconds.
        /// </summary>
        /// <value>
        /// The message receive thread timeout milliseconds.
        /// </value>
        public int MessageReceiveThreadTimeoutMilliseconds { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of messages to receive from queue.
        /// </summary>
        /// <value>
        /// The maximum number of messages to receive from queue.
        /// </value>
        public int MaxNumberOfMessages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [create queue].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [create queue]; otherwise, <c>false</c>.
        /// </value>
        public bool CreateQueue { get; set; }
    }
}
