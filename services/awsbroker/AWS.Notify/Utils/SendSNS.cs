using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using AWS.Notify.Enums;
using AWS.Notify.Interfaces;
using Newtonsoft.Json;


namespace AWS.Notify.Utils
{
    public class SendSNS<T> : IQueueSNS<T>
    {
        private readonly AmazonSimpleNotificationServiceClient _client;
        private readonly string _topicArn;

        public SendSNS(AWSCredentials credentials, string topicArn)
        {
            _client = new AmazonSimpleNotificationServiceClient(credentials, RegionEndpoint.SAEast1);
            _topicArn = topicArn;
        }

        public async Task Send(EnumQueueSNS queue, T message)
        {
            var json = JsonConvert.SerializeObject(message);

            var request = new PublishRequest
            {
                TopicArn = _topicArn,
                Message = json
            };

            await _client.PublishAsync(request);
        }
    }
}
