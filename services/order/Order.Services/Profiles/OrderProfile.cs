using AutoMapper;
using Order.Domain.Entities;
using Order.Services.DTO;
using System;

namespace Order.Services.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderLocationDTO, OrderLocation>();
            CreateMap<OrderLocation, OrderLocationDTO>();
            CreateMap<DateTime, DateOnly>().ConvertUsing(dt => DateOnly.FromDateTime(dt));
            CreateMap<DateOnly, DateTime>().ConvertUsing(dt => new DateTime(dt.Year, dt.Month, dt.Day));
        }
    }
}
