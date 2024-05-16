
using AutoMapper;
using Notify.Domain.Entities;
using Notify.Services.DTO;
namespace Notify.Services.Profiles
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<NotificationDTO, Notification>();
            CreateMap<Notification, NotificationDTO>();
        }
    }
}
