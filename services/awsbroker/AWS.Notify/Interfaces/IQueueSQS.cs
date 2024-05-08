using AWS.Notify.Enums;

namespace AWS.Notify.Interfaces
{
    public interface IQueueSQS<T>
    {
        Task Send(EnumQueueSQS queue, T message);
    }
}
