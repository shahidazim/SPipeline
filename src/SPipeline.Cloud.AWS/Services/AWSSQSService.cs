namespace SPipeline.Cloud.AWS.Services
{
    using Amazon.Runtime;
    using Amazon.SQS;
    using Amazon.SQS.Model;
    using SPipeline.Cloud.AWS.Util;
    using SPipeline.Core.Interfaces.Services;
    using System.Collections.Generic;

    public class AWSSQSService : IQueueService
    {
        private readonly string _serviceUrl;
        private readonly string _accountId;
        private readonly string _queueName;
        private readonly int _maxNumberOfMessages;

        protected AmazonSQSClient queueClient;

        public AWSSQSService(string serviceUrl, string queueName, string accountId, string accessKey, string secretKey, bool createQueue, int maxNumberOfMessages)
        {
            _serviceUrl = serviceUrl;
            _accountId = accountId;
            _queueName = queueName;
            _maxNumberOfMessages = maxNumberOfMessages;

            var awsCredentials = new BasicAWSCredentials(accessKey, secretKey);
            var sqsConfig = new AmazonSQSConfig
            {
                ServiceURL = serviceUrl
            };
            queueClient = new AmazonSQSClient(awsCredentials, sqsConfig);
            if (createQueue)
            {
                CreateQueue(queueName);
            }
        }

        private void CreateQueue(string queueName)
        {
            var response = queueClient.CreateQueue(new CreateQueueRequest(queueName));

            if (AWSResponseValidator.IsValid(response))
            {
                throw new AWSSQSServiceException(response.ToString());
            }
        }

        public IEnumerable<string> Receive()
        {
            var messages = new List<string>();
            var queueUrl = AWSUrlBuilder.CreateSQSUrl(_serviceUrl, _accountId, _queueName).AbsoluteUri;
            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = _maxNumberOfMessages
            };
            while (true)
            {
                var receiveMessageResponse = queueClient.ReceiveMessageAsync(receiveMessageRequest).Result;

                if (AWSResponseValidator.IsValid(receiveMessageResponse))
                {
                    throw new AWSSQSServiceException(receiveMessageResponse.ToString());
                }

                if (receiveMessageResponse.Messages.Count == 0)
                {
                    break;
                }

                foreach (var message in receiveMessageResponse.Messages)
                {
                    messages.Add(message.Body);

                    var deleteMessageResponse =
                        queueClient.DeleteMessage(new DeleteMessageRequest(queueUrl, message.ReceiptHandle));
                    if (AWSResponseValidator.IsValid(deleteMessageResponse))
                    {
                        throw new AWSSQSServiceException(deleteMessageResponse.ToString());
                    }
                }
            }
            return messages;
        }

        public void Send(string content)
        {
            var sendMessageRequest = new SendMessageRequest
            {
                MessageBody = content,
                QueueUrl = AWSUrlBuilder.CreateSQSUrl(_serviceUrl, _accountId, _queueName).AbsoluteUri
            };

            var response = queueClient.SendMessage(sendMessageRequest);

            if (AWSResponseValidator.IsValid(response))
            {
                throw new AWSSQSServiceException(response.ToString());
            }
        }
    }
}
