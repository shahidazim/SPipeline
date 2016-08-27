namespace SPipeline.Cloud.Azure
{
    using Microsoft.ServiceBus.Messaging;
    using SPipeline.Core.Interfaces;
    using SPipeline.Core.Models;
    using SPipeline.Core.Serializers;
    using System;
    using System.IO;
    using System.Text;

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
            : base(configuration.ConnectionString,
                   configuration.QueueName,
                   configuration.MessageTimeToLive,
                   configuration.MaxSizeInMegabytes,
                   configuration.CreateQueue)
        {
        }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <typeparam name="TMessageResponse">The type of the message response.</typeparam>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public IMessageResponse Send<TMessageResponse>(IMessageRequest message)
            where TMessageResponse : IMessageResponse, new()
        {
            var payload = SerializerJson.Serialize(message);
            var payloadStream = new MemoryStream(Encoding.UTF8.GetBytes(payload));
            return Send<TMessageResponse>(new BrokeredMessage(payloadStream, true));
        }

        /// <summary>
        /// Sends the specified brokered message.
        /// </summary>
        /// <param name="brokeredMessage">The brokered message.</param>
        /// <returns></returns>
        private IMessageResponse Send<TMessageResponse>(BrokeredMessage brokeredMessage)
            where TMessageResponse : IMessageResponse, new()
        {
            var response = new TMessageResponse();
            try
            {
                QueueClient.Send(brokeredMessage);
            }
            catch (Exception ex)
            {
                response.AddError(new MessageError(ex, false));
            }

            return response;
        }
    }
}
