﻿using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMq.Notify.DataModels;
using RabbitMq.Notify.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Vehicle.Domain.Entities;
using Vehicle.Infra.Interfaces;
using Vehicle.Infra.Models;

namespace Vehicle.Infra.Messages
{
    public class MotoRpcListener
    {

        private readonly IRabbitConnection _persistentConnection;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public MotoRpcListener(IRabbitConnection persistentConnection, IServiceScopeFactory factory)
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
                List<Moto> clients = new List<Moto>();
                if (ea.RoutingKey == "publishMoto")
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var _motoRepository = scope.ServiceProvider.GetRequiredService<IMotoRepository>();

                        string method = data!.Method;
                        var motoId = 0;
                        Moto? moto = null;
                        switch (method)
                        {
                            case "GetMotoId":
                                motoId = data.Payload["Id"];
                                moto = await _motoRepository.Get(motoId);
                                if (moto != null)
                                {
                                    clients.Add(moto);
                                }
                                response = JsonConvert.SerializeObject(new Response() { Success = true, Payload = clients.FirstOrDefault()! });
                                break;
                            case "GetMotoFilter":
                                var filters = JsonConvert.DeserializeObject<MotoFilters>(data.Payload["Filters"].ToString());
                                clients = await _motoRepository.GetAll(filters);
                                response = JsonConvert.SerializeObject(new Response() { Success = true, Payload = clients.FirstOrDefault()! }); ;
                                break;
                            case "UpdateMoto":
                                motoId = data.Payload["Id"];
                                var isLocated = Convert.ToBoolean(data.Payload["Located"]);

                                moto = await _motoRepository.Get(motoId);

                                moto.Located = isLocated;

                                var nMoto = await _motoRepository.Update(moto);

                                var ret = new { IsUpdate = nMoto.Located == isLocated };
                                response = JsonConvert.SerializeObject(new Response() { Success = true, Payload = ret });


                                break;
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
