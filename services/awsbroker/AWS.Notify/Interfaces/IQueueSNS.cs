using AWS.Notify.Enums;

namespace AWS.Notify.Interfaces
{
    public interface IQueueSNS<T>
    {
        Task Send(EnumQueueSNS queue, T message);

    }
}
