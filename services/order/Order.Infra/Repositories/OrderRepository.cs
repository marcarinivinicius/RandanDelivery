using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities;
using Order.Infra.Context;
using Order.Infra.Interfaces;
using Order.Infra.Models;


namespace Order.Infra.Repositories
{
    public class OrderRepository(OrderContext context) : BaseRepository<OrderLocation>(context), IOrderRepository
    {
        public async Task<List<OrderLocation>> GetAll(OrderFilters? filters = null)
        {
            IQueryable<OrderLocation> query = context.Orders.AsNoTracking();

            if (filters != null)
            {
                if (!filters.AllRecords)
                {
                    if (filters.Active)
                    {
                        query = query.Where(x => x.Active);
                    }
                    else
                    {
                        query = query.Where(moto => moto.Active == false);
                    }
                }

            }

            var orders = await query.ToListAsync();
            return orders;
        }
    }
}
