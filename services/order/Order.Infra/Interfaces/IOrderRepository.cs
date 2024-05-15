using Order.Domain.Entities;
using Order.Infra.Models;

namespace Order.Infra.Interfaces
{
    public interface IOrderRepository : IBaseRepository<OrderLocation>
    {
        Task<List<OrderLocation>> GetAll(OrderFilters? filters = null);
    }
}
