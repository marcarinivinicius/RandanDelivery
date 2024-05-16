using Notify.Domain.Entities;

namespace Notify.Infra.Interfaces
{
    public interface INotificationRepository : IBaseRepository<Notification>
    {
        Task<List<Notification>> GetAll();
    }
}
