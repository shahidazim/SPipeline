namespace SPipeline.Cloud.Azure
{
    using Microsoft.ServiceBus.Messaging;
    using SPipeline.Core.Constants;
    using SPipeline.Core.Interfaces;
    using System.Xml;

    /// <summary>
    /// The Azure Service Bus Sender wrapper to send messages
    /// </summary>
    /// <seealso cref="SPipeline.Cloud.Azure.AzureServiceBusBase" />
    /// <seealso cref="SPipeline.Core.Interfaces.IMessageSender" />
    public class AzureServiceBusSender : AzureServiceBusBase, IMessageSender
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusSender"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public AzureServiceBusSender(AzureServiceBusSenderConfiguration configuration)
        : base(configuration.ConnectionString, configuration.QueueName, configuration.MessageTimeToLive, configuration.MaxSizeInMegabytes)
        {
        }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Send(IMessageRequest message)
        {
            Send(new BrokeredMessage(message)
            {
                ContentType = message.GetType().AssemblyQualifiedName
            });
        }

        /// <summary>
        /// Sends the specified XML message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Send(XmlDocument message)
        {
            Send(new BrokeredMessage(message)
            {
                ContentType = MessageConstants.XmlContentType
            });
        }

        /// <summary>
        /// Sends the specified brokered message.
        /// </summary>
        /// <param name="brokeredMessage">The brokered message.</param>
        private void Send(BrokeredMessage brokeredMessage)
        {
            QueueClient.Send(brokeredMessage);
        }
    }
}
