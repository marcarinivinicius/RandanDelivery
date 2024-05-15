using Newtonsoft.Json;

namespace User.Infra.Messages
{
    public class SqsConsumer
    {
        private readonly IAmazonSQS _sqsClient;

        public SqsConsumer(IAmazonSQS sqsClient)
        {
            _sqsClient = sqsClient;
        }

        public async Task Consume(string queueName, string queueUrl)
        {
            var request = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = 1, // Max number of messages to retrieve at once
                WaitTimeSeconds = 20 // Long polling timeout (in seconds)
            };

            var response = await _sqsClient.ReceiveMessageAsync(request);

            foreach (var message in response.Messages)
            {
                var messageBody = message.Body;
                var messageReceiptHandle = message.ReceiptHandle;

                // Process message
                await ProcessMessageAsync(messageBody);

                // Delete message from the queue
                await DeleteMessageAsync(queueUrl, messageReceiptHandle);
            }
        }

        private async Task ProcessMessageAsync(string messageBody)
        {
            // Deserialize message body
            var messageObject = JsonConvert.DeserializeObject<dynamic>(messageBody);
            // Process the message as needed
            Console.WriteLine("Received message: " + messageObject);
            // Example: You can put your database communication logic here
        }

        private async Task DeleteMessageAsync(string queueUrl, string receiptHandle)
        {
            var request = new DeleteMessageRequest
            {
                QueueUrl = queueUrl,
                ReceiptHandle = receiptHandle
            };

            await _sqsClient.DeleteMessageAsync(request);
            Console.WriteLine("Message deleted from queue.");
        }
    }
}
