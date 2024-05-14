using Order.Infra.Models;
using Order.Services.DTO;

namespace Order.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderLocationDTO> Create(OrderLocationDTO orderDTO);
        Task<OrderLocationDTO> Update(OrderLocationDTO orderDTO);
        Task<OrderLocationDTO> Cancel(OrderLocationCancelDTO cancelOrderDTO);
        Task<OrderLocationDTO> Get(long id);
        Task<List<OrderLocationDTO>> GetAll(OrderFilters filters);

    }
}
