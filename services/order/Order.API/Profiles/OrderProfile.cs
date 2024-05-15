using AutoMapper;
using Order.API.ViewModels.Order;
using Order.Services.DTO;


namespace Order.API.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderViewModel, OrderLocationDTO>();
            CreateMap<UpdateOrderViewModel, OrderLocationDTO>();
            CreateMap<CancelOrderViewModel, OrderLocationCancelDTO>();
        }
    }
}
