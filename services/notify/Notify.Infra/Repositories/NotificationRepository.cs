using Microsoft.EntityFrameworkCore;
using Notify.Domain.Entities;
using Notify.Infra.Context;
using Notify.Infra.Interfaces;

namespace Notify.Infra.Repositories
{
    public class NotificationRepository(NotificationContext context) : BaseRepository<Notification>(context), INotificationRepository
    {
        public async Task<List<Notification>> GetAll()
        {
            IQueryable<Notification> query = context.Notifications.AsNoTracking();
            query = query.OrderByDescending(n => n.Id);

            var motos = await query.ToListAsync();
            return motos;
        }
    }
}
