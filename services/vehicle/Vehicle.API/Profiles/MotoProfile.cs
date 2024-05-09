using AutoMapper;
using Vehicle.API.ViewModels.Moto;
using Vehicle.Services.DTO;

namespace Vehicle.API.Profiles
{
    public class MotoProfile : Profile
    {
        public MotoProfile() {
            CreateMap<CreateMotoViewModel, MotoDTO>();
            CreateMap<UpdateMotoViewModel, MotoDTO>();

        }
    }
}
