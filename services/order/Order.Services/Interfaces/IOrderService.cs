using Order.Infra.Models;
using Order.Services.DTO;

namespace Order.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderLocationDTO> Create(OrderLocationDTO client);
        Task<OrderLocationDTO> Update(OrderLocationDTO client);
        Task Remove(long id);
        Task<OrderLocationDTO> Get(long id);
        Task<List<OrderLocationDTO>> GetAll(OrderFilters filters);

    }
}
