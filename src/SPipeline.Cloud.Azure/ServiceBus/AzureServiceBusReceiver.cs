namespace SPipeline.Cloud.Azure.ServiceBus
{
    using Microsoft.ServiceBus.Messaging;
    using SPipeline.Core.DebugHelper;
    using SPipeline.Core.Interfaces.Pipeline;
    using SPipeline.Core.Serializers;
    using System;
    using System.IO;

    /// <summary>
    /// The Azure Service Bus Receiver wrapper to receive and deserialize the messages
    /// </summary>
    /// <seealso cref="Core.Interfaces.Pipeline.IMessageReceiver" />
    public class AzureServiceBusReceiver : AzureServiceBusBase, IMessageReceiver
    {
        private readonly IMessageDispatcher _messageDispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusReceiver" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="messageDispatcher">The message dispatcher.</param>
        /// <param name="loggerService">The logger service.</param>
        public AzureServiceBusReceiver(AzureServiceBusReceiverConfiguration configuration, IMessageDispatcher messageDispatcher, ILoggerService loggerService)
            : base(configuration.ConnectionString, configuration.QueueName, configuration.CreateQueue, loggerService)
        {
            _messageDispatcher = messageDispatcher;
        }

        /// <summary>
        /// Receiver messages from queue and process them
        /// </summary>
        public void Process()
        {
            while (true)
            {
                var receivedMessage = queueClient.ReceiveAsync().Result;

                if (receivedMessage == null)
                {
                    break;
                }

                try
                {
                    var message = GetBody(receivedMessage);
                    var response = _messageDispatcher.Execute(message);

                    if (response.HasError)
                    {
                        loggerService?.Error(response.GetFormattedError());
                    }

                    receivedMessage.Complete();
                }
                catch (Exception ex)
                {
                    loggerService?.Exception(ex);
                    receivedMessage.Abandon();
                }
            }
        }

        /// <summary>
        /// Deserialized the message body from brokered message.
        /// </summary>
        /// <param name="brokeredMessage">The brokered message.</param>
        /// <returns></returns>
        private IMessageRequest GetBody(BrokeredMessage brokeredMessage)
        {
            var stream = brokeredMessage.GetBody<Stream>();
            var reader = new StreamReader(stream);
            var messageBody = reader.ReadToEnd();
            return (IMessageRequest)SerializerJson.Deserialize(messageBody);
        }
    }
}
