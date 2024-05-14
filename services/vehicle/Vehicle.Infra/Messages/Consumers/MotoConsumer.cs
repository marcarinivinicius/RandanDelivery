using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using RabbitMq.Notify.Interfaces;

public class MotoConsumer
{
    private readonly IRabbitConnection _persistentConnection;

    public MotoConsumer(IRabbitConnection persistentConnection)
    {
        _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
    }

    public void Consume(string queueName)
    {
        if (!_persistentConnection.IsConnected)
            _persistentConnection.TryConnect();

        using (var channel = _persistentConnection.CreateChannel())
        {
            // Declaração da fila com opções adicionais
            var args = new Dictionary<string, object> { { "x-single-active-consumer", true } };
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: args);

            // Configuração da Qualidade de Serviço (QoS)
            channel.BasicQos(0, 1, true);

            // Configuração do consumidor
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArgs) =>
            {
                HandleMessage(eventArgs, channel);
            };

            // Início do consumo de mensagens
            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }
    }

    private void HandleMessage(BasicDeliverEventArgs eventArgs, IModel channel)
    {
        string customRetryHeaderName = "number-of-retries";
        int retryCount = GetRetryCount(eventArgs.BasicProperties, customRetryHeaderName);
        string message = Encoding.UTF8.GetString(eventArgs.Body.Span);

        try
        {
            var data = JsonConvert.DeserializeObject<dynamic>(message);
            var response = new HttpResponseMessage();

            if (eventArgs.RoutingKey == "direct")
            {
                // Lógica de processamento para rota 'direct'
            }

            // Processamento adicional conforme necessário
        }
        catch (Exception ex)
        {
            // Tratamento de erros durante o processamento da mensagem
            HandleProcessingError(ex, eventArgs, channel, customRetryHeaderName, retryCount);
        }
        finally
        {
            // Confirmação da entrega da mensagem
            channel.BasicAck(eventArgs.DeliveryTag, false);
        }
    }

    private void HandleProcessingError(Exception ex, BasicDeliverEventArgs eventArgs, IModel channel, string customRetryHeaderName, int retryCount)
    {
        if (retryCount < 5)
        {
            // Se houver tentativas restantes, reenvia a mensagem com contagem de tentativas atualizada
            IBasicProperties propertiesForCopy = channel.CreateBasicProperties();
            IDictionary<string, object> clone = CloneHeaders(eventArgs.BasicProperties);
            propertiesForCopy.Headers = clone;
            propertiesForCopy.Headers[customRetryHeaderName] = ++retryCount;
            channel.BasicPublish(eventArgs.Exchange, eventArgs.RoutingKey, propertiesForCopy, eventArgs.Body);
        }
        else
        {
            // Se o número máximo de tentativas foi atingido, registrar o erro e descartar a mensagem
            Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
            channel.BasicReject(eventArgs.DeliveryTag, false); // Rejeita a mensagem
        }
    }

    private int GetRetryCount(IBasicProperties properties, string customRetryHeaderName)
    {
        if (properties.Headers != null && properties.Headers.TryGetValue(customRetryHeaderName, out var value))
        {
            return Convert.ToInt32(value);
        }
        return 0;
    }

    private IDictionary<string, object> CloneHeaders(IBasicProperties basicProperties)
    {
        // Implementa a lógica para clonar os cabeçalhos das propriedades básicas
        // Retorna um novo dicionário de cabeçalhos clonados
        throw new NotImplementedException();
    }

    public void Disconnect()
    {
        _persistentConnection.Dispose();
    }
}
