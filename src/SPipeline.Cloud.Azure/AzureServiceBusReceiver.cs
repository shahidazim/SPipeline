namespace SPipeline.Cloud.Azure
{
    using SPipeline.Core.Constants;
    using SPipeline.Core.Interfaces;
    using System;

    /// <summary>
    /// The Azure Service Bus Receiver wrapper to receive and deserialize the messages
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="SPipeline.Cloud.Azure.AzureServiceBusBase" />
    /// <seealso cref="SPipeline.Core.Interfaces.IMessageReceiver" />
    public class AzureServiceBusReceiver<T> : AzureServiceBusBase, IMessageReceiver
    {
        private readonly IMessageDispatcher _messageDispatcher;
        private readonly IMessageConverter<T> _messageConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusReceiver{T}"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="messageDispatcher">The message dispatcher.</param>
        /// <param name="messageConverter">The message converter.</param>
        public AzureServiceBusReceiver(AzureServiceBusReceiverConfiguration configuration, IMessageDispatcher messageDispatcher, IMessageConverter<T> messageConverter)
        : base(configuration.ConnectionString, configuration.QueueName)
        {
            _messageDispatcher = messageDispatcher;
            _messageConverter = messageConverter;
        }

        /// <summary>
        /// Starts listening to receive the messages from Azure Service Bus queue.
        /// </summary>
        public void Start()
        {
            QueueClient.OnMessage((receivedMessage) =>
            {
                try
                {
                    IMessageRequest message;

                    if (IsContentTypeXml(receivedMessage.ContentType))
                    {
                        var xmlMessage = GetXmlBody(receivedMessage);
                        message = _messageConverter.Convert((T)Convert.ChangeType(xmlMessage, typeof(T)));
                    }
                    else
                    {
                        message = GetBody<IMessageRequest>(receivedMessage);
                    }

                    if (message == null)
                    {
                        return;
                    }

                    _messageDispatcher.Execute(message);
                    receivedMessage.Complete();
                }
                catch (Exception ex)
                {
                    // TODO: Implement error handling
                    receivedMessage.Abandon();
                }
            });
        }

        /// <summary>
        /// Determines whether the content type of message is XML.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>
        ///   <c>true</c> if the content type of message is XML; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsContentTypeXml(string contentType)
        {
            return contentType.Contains(MessageConstants.XmlContentType);
        }

        /// <summary>
        /// Close the Azure Service Bus Queue connection.
        /// </summary>
        public void Stop()
        {
            QueueClient.Close();
        }
    }
}
