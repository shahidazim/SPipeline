namespace SPipeline.Cloud.Azure.ServiceBus
{
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Models;
    using SPipeline.Core.Serializers;
    using System;

    /// <summary>
    /// The Azure Service Bus Queue Sender to send messages
    /// </summary>
    /// <seealso cref="Core.Interfaces.Pipeline.IMessageSender" />
    public class AzureServiceBusQueueSender : AzureServiceBusQueueBase, IMessageSender
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusQueueSender" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="loggerService">The logger service.</param>
        public AzureServiceBusQueueSender(AzureServiceBusSenderQueueConfiguration configuration, ILoggerService loggerService)
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
            var response = new TMessageResponse();
            try
            {
                queueService.Send(SerializerJson.Serialize(message));
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
