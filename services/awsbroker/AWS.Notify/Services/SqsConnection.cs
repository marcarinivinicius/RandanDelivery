using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using AWS.Notify.Interfaces;


namespace AWS.Notify.Services
{
    public class SqsConnection : ISqsConnection, IDisposable
    {
        private readonly IAmazonSQS _sqsClient;

        public SqsConnection(string accessKey, string secretKey, string region)
        {
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            _sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.GetBySystemName(region));
        }

        public IAmazonSQS Client => _sqsClient;

        public void Dispose()
        {
            _sqsClient?.Dispose();
        }
    }
}
