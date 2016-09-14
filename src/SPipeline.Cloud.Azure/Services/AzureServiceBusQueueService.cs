namespace SPipeline.Cloud.Azure.Services
{
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using SPipeline.Core.Interfaces.Services;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public class AzureServiceBusQueueService : IQueueService
    {
        private const int DefaultMaxSizeInMegabytes = 1024;

        private readonly QueueClient _queueClient;

        public AzureServiceBusQueueService(
            string connectionString,
            string queueName,
            bool createQueue = false,
            TimeSpan? messageTimeToLive = null,
            int maxSizeInMegabytes = 0)
        {
            if (createQueue)
            {
                CreateQueue(connectionString, queueName, messageTimeToLive, maxSizeInMegabytes);
            }
            _queueClient = QueueClient.CreateFromConnectionString(connectionString, queueName);
        }

        private void CreateQueue(
            string connectionString,
            string queueName,
            TimeSpan? messageTimeToLive,
            int maxSizeInMegabytes)
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            if (namespaceManager.QueueExists(queueName))
            {
                return;
            }

            var queueDescription = new QueueDescription(queueName)
            {
                MaxSizeInMegabytes = maxSizeInMegabytes == 0 ? DefaultMaxSizeInMegabytes : maxSizeInMegabytes,
            };
            if (messageTimeToLive.HasValue)
            {
                queueDescription.DefaultMessageTimeToLive = messageTimeToLive.Value;
            }
            namespaceManager.CreateQueue(queueDescription);
        }

        public void Send(string content)
        {
            var payloadStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var brokeredMessage = new BrokeredMessage(payloadStream, true);
            _queueClient.Send(brokeredMessage);
        }

        public IEnumerable<string> Receive()
        {
            var messages = new List<string>();
            while (true)
            {
                var receivedMessage = _queueClient.ReceiveAsync().Result;

                if (receivedMessage == null)
                {
                    break;
                }

                try
                {
                    var message = GetBody(receivedMessage);
                    messages.Add(message);
                    receivedMessage.Complete();
                }
                catch
                {
                    receivedMessage.Abandon();
                    throw;
                }
            }
            return messages;
        }

        private string GetBody(BrokeredMessage brokeredMessage)
        {
            var stream = brokeredMessage.GetBody<Stream>();
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
