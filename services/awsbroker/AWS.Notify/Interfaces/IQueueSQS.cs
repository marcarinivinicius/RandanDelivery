
using AWS.Notify.DataModels;

namespace AWS.Notify.Interfaces
{
    public interface IQueueSQS
    {
        Task Send(RequestAws obj, string queue);
    }
}
