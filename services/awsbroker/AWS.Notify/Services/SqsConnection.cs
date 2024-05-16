using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using AWS.Notify.Interfaces;


namespace AWS.Notify.Services
{
    public class SqsConnection : ISqsConnection, IDisposable
    {
        private readonly IAmazonSQS _sqsClient;

        public SqsConnection(BasicAWSCredentials credentials, string region, string urlQueue)
        {
            _sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.GetBySystemName(region));
            UrlQueue = urlQueue;
        }

        public IAmazonSQS Client => _sqsClient;

        public string UrlQueue { get; set; }

        public void Dispose()
        {
            _sqsClient?.Dispose();
        }
    }
}
