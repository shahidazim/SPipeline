namespace SPipeline.Cloud.Azure.Services
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue;
    using SPipeline.Core.Interfaces.Services;
    using System;
    using System.Collections.Generic;

    public class AzureStorageQueueService : IQueueService
    {
        private CloudQueue _queue;
        private readonly int _maxNumberOfMessages;

        public AzureStorageQueueService(
            string connectionString,
            string queueName,
            bool createQueue,
            TimeSpan? messageTimeToLive,
            int maxNumberOfMessages)
        {
            _maxNumberOfMessages = maxNumberOfMessages;

            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();

            _queue = queueClient.GetQueueReference(queueName);

            if (createQueue)
            {
                CreateQueue(queueClient, queueName, messageTimeToLive);
            }
        }

        private void CreateQueue(CloudQueueClient queueClient, string queueName, TimeSpan? messageTimeToLive)
        {
            var queueRequestOptions = new QueueRequestOptions
            {
                MaximumExecutionTime = messageTimeToLive
            };
            _queue.CreateIfNotExists(queueRequestOptions);
        }

        public void Send(string content)
        {
            var message = new CloudQueueMessage(content);
            _queue.AddMessage(message);
        }

        public IEnumerable<string> Receive()
        {
            var messages = new List<string>();
            var receivedMessages = _queue.GetMessages(_maxNumberOfMessages);
            foreach (var receivedMessage in receivedMessages)
            {
                messages.Add(receivedMessage.AsString);
                _queue.DeleteMessage(receivedMessage);
            }
            return messages;
        }
    }
}
