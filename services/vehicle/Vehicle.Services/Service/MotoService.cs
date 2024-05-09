
using AutoMapper;
using RabbitMq.Notify.Services;
using Vehicle.Infra.Interfaces;
using Vehicle.Services.DTO;
using Vehicle.Services.Interfaces;

namespace Vehicle.Services.Service
{
    public class MotoService : IMotoService
    {
        private readonly IMapper _mapper;
        private readonly IMotoRepository _motoRepository;
        private readonly RabbitMqClient _rabbitMqClient;
        public MotoService(IMapper mapper, IMotoRepository motoRepository, RabbitMqClient rabbitMqClient)
        {
            _mapper = mapper;
            _motoRepository = motoRepository;
            _rabbitMqClient = rabbitMqClient;
        }

        public Task<MotoDTO> Create(MotoDTO client)
        {
            throw new NotImplementedException();
        }

        public Task<MotoDTO> Get(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<MotoDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Remove(long id)
        {
            throw new NotImplementedException();
        }

        public Task<MotoDTO> Update(MotoDTO client)
        {
            throw new NotImplementedException();
        }
    }
}
