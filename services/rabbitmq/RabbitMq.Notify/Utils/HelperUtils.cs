using RabbitMQ.Client;

namespace RabbitMq.Notify.Utils
{
    public static class HelperUtils
    {
        public static int GetRetryCountFromMessage(IBasicProperties messageProperties, string countHeader)
        {
            if (messageProperties == null || messageProperties.Headers == null || !messageProperties.Headers.ContainsKey(countHeader))
            {
                return 0;
            }

            if (int.TryParse(Convert.ToString(messageProperties.Headers[countHeader]), out int retryCount))
            {
                return retryCount;
            }

            return 0;
        }

        public static IDictionary<string, object> CloneHeaders(IBasicProperties originalProperties)
        {
            var clonedHeaders = new Dictionary<string, object>();
            if (originalProperties == null || originalProperties.Headers == null)
            {
                return clonedHeaders;
            }

            foreach (var header in originalProperties.Headers)
            {
                clonedHeaders[header.Key] = header.Value;
            }

            return clonedHeaders;
        }

        public static string GenerateUniqueId()
        {
            return Guid.NewGuid().ToString();
        }
        public static int GetQueueLength(IModel channel, string queueName)
        {
            var result = channel.QueueDeclarePassive(queueName);
            return (int)result.MessageCount;
        }
        public static TimeSpan GetExponentialBackoffDelay(int retryCount)
        {
            // Calcula o intervalo de retentativa exponencial, por exemplo, aumentando o intervalo de retentativa a cada tentativa
            var delay = Math.Pow(2, retryCount);
            // Adiciona um jitter aleatório para evitar picos de tráfego sincronizado
            delay += new Random().Next(0, 1000); // Adiciona um jitter de até 1000ms
            return TimeSpan.FromMilliseconds(delay);
        }

    }
}
