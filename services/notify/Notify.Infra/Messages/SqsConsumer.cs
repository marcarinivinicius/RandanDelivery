using Amazon.SQS;
using Amazon.SQS.Model;
using AWS.Notify.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Notify.Domain.Entities;
using Notify.Infra.Interfaces;
using RabbitMq.Notify.DataModels;

namespace Vehicle.Infra.Messages
{
    public class SqsConsumer
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SqsConsumer(ISqsConnection sqsConnection, IServiceScopeFactory factory)
        {
            _sqsClient = sqsConnection.Client;
            _serviceScopeFactory = factory;
        }

        public async Task Consume(string queueUrl)
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
            var data = JsonConvert.DeserializeObject<Request>(messageBody);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _notificationrepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

                string method = data!.Method;

                switch (method)
                {
                    case "fyFabrication":
                        var notify = new Notification(data.Payload);
                        await _notificationrepository.Create(notify);
                        break;
                }

            }


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
