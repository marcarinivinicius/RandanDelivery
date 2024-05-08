
using RabbitMQ.Client;

namespace RabbitMq.Notify.Interfaces
{
    public interface IRabbitConnection : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        IModel CreateChannel();

    }
}
