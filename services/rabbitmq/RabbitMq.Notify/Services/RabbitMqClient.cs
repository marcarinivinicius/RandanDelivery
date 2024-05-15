using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using RabbitMq.Notify.DataModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMq.Notify.Interfaces;
using Newtonsoft.Json;

namespace RabbitMq.Notify.Services
{
    public class RabbitMqClient : IDisposable
    {
        private readonly IModel _channel;
        private readonly string _replyQueueName;
        private readonly BlockingCollection<string> _respQueue = new BlockingCollection<string>();
        private readonly IBasicProperties _props;
        private readonly IRabbitConnection _persistentConnection;
        private readonly EventingBasicConsumer _consumer;

        public RabbitMqClient(IRabbitConnection persistentConnection)
        {
            _persistentConnection = persistentConnection;

            if (!_persistentConnection.IsConnected)
                _persistentConnection.TryConnect();

            _channel = _persistentConnection.CreateChannel();
            _channel.ConfirmSelect();
            _replyQueueName = "reply";

            DeclareQueue();
            _consumer = new EventingBasicConsumer(_channel);
            _props = _channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            _props.CorrelationId = correlationId;
            _props.ReplyTo = _replyQueueName;


            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    _respQueue.Add(response);
                }
            };

            _channel.BasicAcks += (sender, ea) =>
            {
                // Lógica de confirmação de entrega
            };

            _channel.BasicNacks += (sender, ea) =>
            {
                // Lógica de falha na entrega
            };
        }

        private void DeclareQueue()
        {
            var args = new Dictionary<string, object>
        {
            { "x-single-active-consumer", true }
        };

            _channel.QueueDeclare(queue: _replyQueueName, durable: false, exclusive: false, autoDelete: false, arguments: args);
        }

        public Response? Call(Request obj, string queue)
        {
            var message = JsonConvert.SerializeObject(obj);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
               exchange: "",
               routingKey: queue,
               basicProperties: _props,
               body: body);

            _channel.BasicConsume(
               consumer: _consumer,
               queue: _replyQueueName,
               autoAck: true);

            var receivedMessage = _respQueue.Take();

            return JsonConvert.DeserializeObject<Response>(receivedMessage);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _persistentConnection?.Dispose();
        }


    }
}
