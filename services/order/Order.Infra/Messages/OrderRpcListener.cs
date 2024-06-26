﻿using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Order.Domain.Entities;
using Order.Infra.Interfaces;
using RabbitMq.Notify.DataModels;
using RabbitMq.Notify.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


namespace Order.Infra.Messages
{
    public class OrderRpcListener
    {

        private readonly IRabbitConnection _persistentConnection;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public OrderRpcListener(IRabbitConnection persistentConnection, IServiceScopeFactory factory)
        {
            _persistentConnection = persistentConnection;
            _serviceScopeFactory = factory;
        }

        public void Consume(string queue)
        {
            if (!_persistentConnection.IsConnected)
                _persistentConnection.TryConnect();

            var channel = _persistentConnection.CreateChannel();

            var args = new Dictionary<string, object> { { "x-single-active-consumer", true } };

            channel.QueueDeclare(
                   queue: queue,
                   durable: true,
                   exclusive: false,
                   autoDelete: false,
                   arguments: args);
            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
            consumer.Received += async (model, ea) =>
            {
                await ReceivedEventAsync(model!, ea, channel);
                // Simulação de atraso para desacelerar o consumo
                await Task.Delay(TimeSpan.FromSeconds(5));
            };
        }

        private async Task ReceivedEventAsync(object sender, BasicDeliverEventArgs ea, IModel channel)
        {
            string response = null;

            var props = ea.BasicProperties;
            var replyProps = channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            try
            {
                var message = Encoding.UTF8.GetString(ea.Body.Span);
                var data = JsonConvert.DeserializeObject<Request>(message);
                List<OrderLocation> clients = new List<OrderLocation>();
                if (ea.RoutingKey == "publishOrder")
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var _orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                        string method = data!.Method;
                        switch (method)
                        {
                            //case "GetMotoId":
                            //    int motoId = data.Payload["Id"];

                            //    var moto = await _motoRepository.Get(motoId);
                            //    if (moto != null)
                            //    {
                            //        clients.Add(moto);
                            //    }
                            //    response = JsonConvert.SerializeObject(new Response() { Success = true, Payload = clients.FirstOrDefault()! });
                            //    break;
                            //case "GetMotoFilter":
                            //    var filters = JsonConvert.DeserializeObject<OrderFilters>(data.Payload["Filters"].ToString());
                            //    var motos = await _motoRepository.GetAll(filters);
                            //    clients.AddRange(motos);
                            //    response = JsonConvert.SerializeObject(new Response() { Success = true, Payload = clients.FirstOrDefault()! });
                            //    break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //logging
                response = JsonConvert.SerializeObject(new Response() { Success = false, ErrMessage = ex.Message });
            }
            finally
            {
                if (!string.IsNullOrEmpty(response))
                {
                    var responseBytes = Encoding.UTF8.GetBytes(response!);
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: props.ReplyTo,
                        basicProperties: replyProps,
                        body: responseBytes);
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                else
                {
                    channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: true);
                }

            }
        }

        public void Disconnect()
        {
            _persistentConnection.Dispose();
        }
    }
}
