using Notify.Services.DTO;

namespace Notify.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationDTO>> GetAll();

    }
}
