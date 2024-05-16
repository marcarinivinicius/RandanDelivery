using Amazon.SQS;
using Amazon.SQS.Model;
using AWS.Notify.DataModels;
using AWS.Notify.Interfaces;
using Newtonsoft.Json;

namespace AWS.Notify.Utils
{
    public class SendSQS : IQueueSQS
    {
        private readonly IAmazonSQS _client;
        private readonly string _urlQueue;

        public SendSQS(ISqsConnection sqsConnection)
        {
            _client = sqsConnection.Client;
            _urlQueue = sqsConnection.UrlQueue;
        }

        public async Task Send(RequestAws obj, string queue)
        {
            var message = JsonConvert.SerializeObject(obj);

            var request = new SendMessageRequest
            {
                QueueUrl = $"{_urlQueue}/{queue}",
                MessageBody = message
            };

            await _client.SendMessageAsync(request);
        }
    }
}
