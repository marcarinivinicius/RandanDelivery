using Newtonsoft.Json;
using RabbitMq.Notify.Interfaces;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using RabbitMq.Notify.Utils;

namespace Vehicle.Infra.Messages.Consumers
{
    public class MotoConsumer
    {
        private readonly IRabbitConnection _persistentConnection;
        public MotoConsumer(IRabbitConnection persistentConnection)
        {
            _persistentConnection = persistentConnection;
        }

        public void Consume(string queue)
        {

            if (!_persistentConnection.IsConnected)
                _persistentConnection.TryConnect();

            var channel = _persistentConnection.CreateChannel();
            var args = new Dictionary<string, object> { { "x-single-active-consumer", true } };

            channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: args);

            // BasicQos method uses which to make it possible to limit the number of unacknowledged messages on a channel.
            channel.BasicQos(0, 1, true);
            var consumer = new EventingBasicConsumer(channel);

            BasicGetResult result = channel.BasicGet(queue, false);
            channel.BasicRecover(true);
            consumer.Received += (ch, ea) =>
            {
                if (ch != null) ReceivedEvent(ch, ea, channel);
            };

            consumer.Shutdown += (o, e) =>
            {
                //logging
            };

            channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
        }

        private static void ReceivedEvent(object sender, BasicDeliverEventArgs e, IModel channel)
        {
            string customRetryHeaderName = "number-of-retries";
            int retryCount = HelperUtils.GetRetryCountFromMessage(e.BasicProperties, customRetryHeaderName);
            var message = Encoding.UTF8.GetString(e.Body.Span);

            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(message);
                var response = new HttpResponseMessage();

                if (e.RoutingKey == "direct")
                {
                    //
                }
            }
            catch
            {
                if (retryCount != 5)
                {
                    IBasicProperties propertiesForCopy = channel.CreateBasicProperties();
                    IDictionary<string, object> clone = HelperUtils.CloneHeaders(e.BasicProperties);
                    propertiesForCopy.Headers = clone;
                    propertiesForCopy.Headers[customRetryHeaderName] = ++retryCount;
                    channel.BasicPublish(e.Exchange, e.RoutingKey, propertiesForCopy, e.Body);
                }
                else
                {
                    // logging
                }
            }
            finally
            {
                channel.BasicAck(e.DeliveryTag, false);
            }
        }

        public void Disconnect()
        {
            _persistentConnection.Dispose();
        }
    }
}
