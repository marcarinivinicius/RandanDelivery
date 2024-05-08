using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using AWS.Notify.Enums;
using AWS.Notify.Interfaces;
using Newtonsoft.Json;

namespace AWS.Notify.Utils
{
    public class SendSQS<T> : IQueueSQS<T>
    {
        private readonly AmazonSQSClient _client;
        private readonly string _queueUrl;

        public SendSQS(AWSCredentials credentials, string queueUrl)
        {
            // Aqui você pode inicializar o cliente AmazonSQSClient
            _client = new AmazonSQSClient(credentials, RegionEndpoint.SAEast1);
            _queueUrl = queueUrl;
        }

        public async Task Send(EnumQueueSQS queue, T message)
        {
            var json = JsonConvert.SerializeObject(message);

            var request = new SendMessageRequest
            {
                QueueUrl = _queueUrl,
                MessageBody = json
            };

            await _client.SendMessageAsync(request);
        }
    }
}
