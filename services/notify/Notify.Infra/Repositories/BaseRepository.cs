using Microsoft.EntityFrameworkCore;
using Notify.Domain.Entities;
using Notify.Infra.Context;
using Notify.Infra.Interfaces;


namespace Notify.Infra.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : ModelBase
    {
        private readonly NotificationContext _context;
        public BaseRepository(NotificationContext context)
        {
            _context = context;
        }
        async Task<T> IBaseRepository<T>.Create(T obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        async Task IBaseRepository<T>.Delete(long id)
        {
            var users = await _context.Set<T>().AsNoTracking().Where(x => x.Id == id).ToListAsync();
            var user = users.FirstOrDefault();
            if (user != null)
            {
                _context.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        async Task<T> IBaseRepository<T>.Get(long id)
        {
            var obj = await _context.Set<T>()
                .AsNoTracking()
                .Where(x => x.Id == id).ToListAsync();
            return obj.FirstOrDefault()!;
        }

        async Task<T> IBaseRepository<T>.Update(T obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return obj;
        }
    }
}

