using Polly;
using RabbitMq.Notify.Interfaces;
using RabbitMq.Notify.Utils;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMq.Notify.Services
{
    public class RabbitProducer
    {
        private readonly IRabbitConnection _persistentConnection;
        private readonly ILoggerAdapter<RabbitProducer> _logger;

        public RabbitProducer(IRabbitConnection persistentConnection, ILoggerAdapter<RabbitProducer> logger)
        {
            _persistentConnection = persistentConnection;
            _logger = logger;

            if (!_persistentConnection.IsConnected)
            {
                TryConnect();
            }
        }

        public void Publish(string queueName, string message = null, int retryCount = 1)
        {
            if (!_persistentConnection.IsConnected)
            {
                _logger.LogWarning("Tentativa de publicação sem conexão RabbitMQ estabelecida.");
                return;
            }

            var retryPolicy = GetRetryPolicy(retryCount);

            using var channel = _persistentConnection.CreateChannel();

            DeclareQueue(channel, queueName);

            retryPolicy.Execute(() =>
            {
                try
                {
                    PublishMessage(channel, queueName, message, retryCount);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Falha ao publicar mensagem no RabbitMQ.");
                    throw;
                }
            });
        }

        private void TryConnect()
        {
            try
            {
                _persistentConnection.TryConnect();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao conectar ao RabbitMQ durante a inicialização do produtor.");
            }
        }

        private void DeclareQueue(IModel channel, string queueName)
        {
            var args = new Dictionary<string, object>
        {
            { "x-single-active-consumer", true }
        };

            channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: args);
        }

        private void PublishMessage(IModel channel, string queueName, string message, int retryCount)
        {
            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.DeliveryMode = 2;

            properties.Headers ??= new Dictionary<string, object>();
            properties.Headers["RetryCount"] = retryCount;

            channel.ConfirmSelect();
            channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                mandatory: true,
                basicProperties: properties,
                body: body);

            channel.WaitForConfirmsOrDie();

            _logger.LogInformation("Mensagem publicada com sucesso no RabbitMQ.");
        }

        private Policy GetRetryPolicy(int retryCount)
        {
            return Policy.Handle<Exception>()
                .WaitAndRetry(retryCount, retryAttempt => HelperUtils.GetExponentialBackoffDelay(retryAttempt), (ex, time) =>
                {
                    _logger.LogError(ex, "Tentativa de publicação falhou. Tentando novamente em {Delay} segundos.", time.TotalSeconds);
                });
        }


    }
}
