using Microsoft.EntityFrameworkCore;
using User.Infra.Interfaces;
using Vehicle.Domain.Entities;
using Vehicle.Infra.Context;


namespace Vehicle.Infra.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : ModelBase
    {
        private readonly VehicleContext _context;
        public BaseRepository(VehicleContext context)
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

