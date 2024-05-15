using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMq.Notify.DataModels;
using RabbitMq.Notify.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using User.Domain.Entities;
using User.Infra.Context;

namespace User.Infra.Messages
{
    public class UserRpcListener
    {

        private readonly IRabbitConnection _persistentConnection;
        private readonly IDbContextFactory<UserContext> _context;
        public UserRpcListener(IRabbitConnection persistentConnection, IServiceScopeFactory factory, IDbContextFactory<UserContext> context)
        {
            _persistentConnection = persistentConnection;
            _context = context;
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
                if (ea.RoutingKey == "publishUser")
                {
                    var dbContext = _context.CreateDbContext();
                    string method = data!.Method;
                    List<Client> clients = new List<Client>();
                    switch (method)
                    {
                        case "GetUser":
                            int userId = data.Payload["Id"];
                            clients = await dbContext.Clients.AsNoTracking().Where(x => x.Id == userId).ToListAsync();
                            response = JsonConvert.SerializeObject(new Response() { Success = true, Payload = clients.FirstOrDefault()! });
                            break;
                        case "GetUserLogged":
                            string email = data.Payload["Email"];
                            clients = await dbContext.Clients.AsNoTracking().Where(x => x.Email == email).ToListAsync();
                            var client = clients.FirstOrDefault();
                            if (client != null)
                            {
                                // Refatorar o Redis, mover para infra, reestruturar a conexão para buscar aqui se tem autenticação logada para esse usuário; 
                                // devolver usuário se tiver token autenticado ainda, se não remover.
                                response = JsonConvert.SerializeObject(new Response() { Success = true, Payload = clients.FirstOrDefault()! });
                            }
                            break;
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
