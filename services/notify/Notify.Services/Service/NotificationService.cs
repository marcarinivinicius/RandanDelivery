
using AutoMapper;
using Notify.Infra.Interfaces;
using Notify.Services.DTO;
using Notify.Services.Interfaces;


namespace Notify.Services.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IMapper _mapper;
        private readonly INotificationRepository _notificationRepository;
        public NotificationService(IMapper mapper, INotificationRepository notificationRepository)
        {
            _mapper = mapper;
            _notificationRepository = notificationRepository;
        }

        public async Task<List<NotificationDTO>> GetAll()
        {
            var notification = await _notificationRepository.GetAll();

            return _mapper.Map<List<NotificationDTO>>(notification);
        }

    }
}
