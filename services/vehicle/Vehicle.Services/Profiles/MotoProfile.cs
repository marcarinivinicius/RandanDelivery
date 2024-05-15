
using AutoMapper;
using Vehicle.Domain.Entities;
using Vehicle.Services.DTO;

namespace Vehicle.Services.Profiles
{
    public class MotoProfile : Profile
    {
        public MotoProfile()
        {
            CreateMap<MotoDTO, Moto>();
            CreateMap<Moto, MotoDTO>();
        }
    }
}
