using AutoMapper;
using System.Globalization;
using Vehicle.API.ViewModels.Moto;
using Vehicle.Services.DTO;

namespace Vehicle.API.Profiles
{
    public class MotoProfile : Profile
    {
        public MotoProfile()
        {
            CreateMap<CreateMotoViewModel, MotoDTO>()
                .ForMember(dest => dest.Fabrication, opt => opt.MapFrom(src => ParseFabrication(src.Fabrication)));
            CreateMap<UpdateMotoViewModel, MotoDTO>()
                .ForMember(dest => dest.Fabrication, opt => opt.MapFrom(src => ParseFabrication(src.Fabrication)));

        }
        private DateOnly ParseFabrication(string fabricationYear)
        {
            if (DateTime.TryParseExact(fabricationYear, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return new DateOnly(result.Year, 1, 1);
            }

            throw new ArgumentException("Invalid fabrication year format.");
        }
    }
}
