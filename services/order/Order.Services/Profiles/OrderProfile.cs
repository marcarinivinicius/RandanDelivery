using AutoMapper;
using Order.Domain.Entities;
using Order.Services.DTO;

namespace Order.Services.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderLocationDTO, OrderLocation>();
            CreateMap<OrderLocation, OrderLocationDTO>();
        }
    }
}
