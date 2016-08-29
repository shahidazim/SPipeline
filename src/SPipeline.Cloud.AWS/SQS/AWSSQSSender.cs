namespace SPipeline.Cloud.AWS.SQS
{
    using Amazon.SQS.Model;
    using SPipeline.Cloud.AWS.Util;
    using SPipeline.Core.Interfaces;
    using SPipeline.Core.Models;
    using SPipeline.Core.Serializers;
    using System;
    using System.Net;

    public class AWSSQSSender : AWSSQSBase, IMessageSender
    {
        public AWSSQSSender(AWSSQSSenderConfiguration configuration)
            : base(configuration.ServiceUrl, configuration.QueueName, configuration.AccountId, configuration.AccessKey, configuration.SecretKey, configuration.CreateQueue)
        {
        }

        public IMessageResponse Send<TMessageResponse>(IMessageRequest message)
            where TMessageResponse : IMessageResponse, new()
        {
            var response = new TMessageResponse();
            try
            {
                var sendMessageRequest = new SendMessageRequest
                {
                    MessageBody = SerializerJson.Serialize(message),
                    QueueUrl = AWSQueryUrlBuilder.Create(serviceUrl, accountId, queueName)
                };

                var sqsResponse = queueClient.SendMessage(sendMessageRequest);

                if (sqsResponse.HttpStatusCode != HttpStatusCode.OK)
                {
                    response.AddError(new MessageError(sqsResponse.ToString(), false));
                }
            }
            catch (Exception ex)
            {
                response.AddError(new MessageError(ex, false));
            }

            return response;
        }
    }
}
