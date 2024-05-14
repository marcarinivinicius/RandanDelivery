using AutoMapper;
using Order.Domain.Entities;
using Order.Domain.Exceptions;
using Order.Infra.Interfaces;
using Order.Infra.Models;
using Order.Services.DTO;
using Order.Services.Interfaces;
using RabbitMq.Notify.Services;


namespace Order.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IMapper mapper, IOrderRepository orderRepository)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async Task<OrderLocationDTO> Create(OrderLocationDTO OrderLocationDTO)
        {
            //OrderFilters filters = new()
            //{
            //    PlateCode = OrderLocationDTO.PlateCode
            //};
            //var _hasMoto = await _orderRepository.GetAll(filters);
            //if (_hasMoto != null && _hasMoto.Count() > 0) throw new PersonalizeExceptions("This vehicle is already registered");

            var moto = _mapper.Map<OrderLocation>(OrderLocationDTO);
            moto.Validate();
            var nMoto = await _orderRepository.Create(moto);
            return _mapper.Map<OrderLocationDTO>(nMoto);
        }

        public async Task<OrderLocationDTO> Update(OrderLocationDTO OrderLocationDTO)
        {
            var _hasMoto = await _orderRepository.Get(OrderLocationDTO.Id);
            if (_hasMoto == null) throw new PersonalizeExceptions("Vehicle not found");
            var moto = _mapper.Map<OrderLocation>(OrderLocationDTO);
            moto.Validate();
            var nMoto = await _orderRepository.Update(moto);

            return _mapper.Map<OrderLocationDTO>(nMoto);
        }

        public async Task Remove(long id)
        {
            await _orderRepository.Delete(id);
        }

        public async Task<OrderLocationDTO> Get(long id)
        {
            var OrderLocationDTO = await _orderRepository.Get(id);
            return _mapper.Map<OrderLocationDTO>(OrderLocationDTO);
        }

        public async Task<List<OrderLocationDTO>> GetAll(OrderFilters filters)
        {
            var motosDTO = await _orderRepository.GetAll(filters);

            return _mapper.Map<List<OrderLocationDTO>>(motosDTO);
        }

    }
}
