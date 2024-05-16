
using AutoMapper;
using RabbitMq.Notify.Services;
using Vehicle.Domain.Entities;
using Vehicle.Domain.Exceptions;
using Vehicle.Infra.Interfaces;
using Vehicle.Infra.Models;
using Vehicle.Services.DTO;
using Vehicle.Services.Interfaces;

namespace Vehicle.Services.Service
{
    public class MotoService : IMotoService
    {
        private readonly IMapper _mapper;
        private readonly IMotoRepository _motoRepository;
        private readonly INotificationService _notificationService;
        public MotoService(IMapper mapper, IMotoRepository motoRepository, INotificationService notificationService)
        {
            _mapper = mapper;
            _motoRepository = motoRepository;
            _notificationService = notificationService;
        }

        public async Task<MotoDTO> Create(MotoDTO motoDTO)
        {
            MotoFilters filters = new()
            {
                PlateCode = motoDTO.PlateCode
            };
            var _hasMoto = await _motoRepository.GetAll(filters);
            if (_hasMoto != null && _hasMoto.Count() > 0) throw new PersonalizeExceptions("This vehicle is already registered");

            var moto = _mapper.Map<Moto>(motoDTO);
            moto.Validate();
            var nMoto = await _motoRepository.Create(moto);

            var nmotoDTO = _mapper.Map<MotoDTO>(nMoto);

            if (nmotoDTO.Fabrication.Year == 2024)
            {
                await _notificationService.SendNewMoto(motoDTO);
            }
            return nmotoDTO;
        }

        public async Task<MotoDTO> Update(MotoDTO motoDTO)
        {
            var _hasMoto = await _motoRepository.Get(motoDTO.Id);
            if (_hasMoto == null) throw new PersonalizeExceptions("Vehicle not found");
            var moto = _mapper.Map<Moto>(motoDTO);
            moto.Validate();
            var nMoto = await _motoRepository.Update(moto);

            return _mapper.Map<MotoDTO>(nMoto);
        }

        public async Task Remove(long id)
        {
            MotoFilters filters = new()
            {
                Id = id,
                AllLocated = false,
                Located = true,
            };
            var _hasMoto = await _motoRepository.GetAll(filters);
            if (_hasMoto == null || _hasMoto.Count() == 0) throw new PersonalizeExceptions("It is not possible to delete a rental vehicle");

            await _motoRepository.Delete(id);
        }

        public async Task<MotoDTO> Get(long id)
        {
            var motoDTO = await _motoRepository.Get(id);
            return _mapper.Map<MotoDTO>(motoDTO);
        }

        public async Task<List<MotoDTO>> GetAll(MotoFilters filters)
        {
            var motosDTO = await _motoRepository.GetAll(filters);

            return _mapper.Map<List<MotoDTO>>(motosDTO);
        }

    }
}
