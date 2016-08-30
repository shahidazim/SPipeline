namespace SPipeline.Cloud.Azure.ServiceBus
{
    using Microsoft.ServiceBus.Messaging;
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Models;
    using SPipeline.Core.Serializers;
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// The Azure Service Bus Sender wrapper to send messages
    /// </summary>
    /// <seealso cref="Core.Interfaces.Pipeline.IMessageSender" />
    public class AzureServiceBusSender : AzureServiceBusBase, IMessageSender
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusSender" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="loggerService">The logger service.</param>
        public AzureServiceBusSender(AzureServiceBusSenderConfiguration configuration, ILoggerService loggerService)
            : base(configuration.ConnectionString,
                   configuration.QueueName,
                   configuration.MessageTimeToLive,
                   configuration.MaxSizeInMegabytes,
                   configuration.CreateQueue,
                   loggerService)
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
                queueClient.Send(brokeredMessage);
            }
            catch (Exception ex)
            {
                loggerService?.Exception(ex);
                response.AddError(new MessageError(ex, false));
            }

            return response;
        }
    }
}
