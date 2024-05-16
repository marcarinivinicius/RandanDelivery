using Amazon.SQS;
using Amazon.SQS.Model;
using AWS.Notify.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMq.Notify.DataModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Vehicle.Domain.Entities;
using Vehicle.Infra.Interfaces;
using Vehicle.Infra.Models;
using Amazon.SimpleNotificationService.Util;

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
                var _motoRepository = scope.ServiceProvider.GetRequiredService<IMotoRepository>();

                string method = data!.Method;

                switch (method)
                {
                    case "fyFabrication":


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
