using RabbitMq.Notify.Interfaces;

namespace RabbitMq.Notify.Services
{
    public class LoggerAdapter<T> : ILoggerAdapter<T>
    {
        public void LogInformation(string message)
        {
        }

        public void LogWarning(string message)
        {
        }

        public void LogError(Exception ex, string message, dynamic? message2 = null)
        {
        }
    }
}
