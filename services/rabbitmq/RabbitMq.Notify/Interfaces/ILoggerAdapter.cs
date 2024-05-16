namespace RabbitMq.Notify.Interfaces
{
    public interface ILoggerAdapter<T>
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(Exception ex, string message, dynamic? message2 = null);
    }
}
