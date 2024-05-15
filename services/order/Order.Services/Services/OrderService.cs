using AutoMapper;
using Order.Domain.Entities;
using Order.Domain.Exceptions;
using Order.Infra.Interfaces;
using Order.Infra.Models;
using Order.Services.DTO;
using Order.Services.Interfaces;


namespace Order.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserService _userService;
        private readonly IMotoService _motoService;

        public OrderService(IMapper mapper, IOrderRepository orderRepository, IUserService userService, IMotoService motoService)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _userService = userService;
            _motoService = motoService;
        }

        public async Task<OrderLocationDTO> Create(OrderLocationDTO orderLocationDTO)
        {
            var clientUser = await _userService.GetUser(orderLocationDTO.RiderId);
            if (!CnhTypeValidation(clientUser.CnhType)) throw new PersonalizeExceptions("This driver does not have a driver's license compatible with rental");

            var motoAvalilable = _motoService.GetMotoAvailable();

            if (motoAvalilable == null) throw new PersonalizeExceptions("There are no motorbikes currently available for rental");

            orderLocationDTO.VehicleId = motoAvalilable.Id;

            var order = _mapper.Map<OrderLocation>(orderLocationDTO);
            order.Validate();

            var setLocated = _motoService.UpdateMoto(order.VehicleId, true);

            if (!setLocated) throw new PersonalizeExceptions("Failed to update the vehicle, try again later");
            try
            {
                var nOrder = await _orderRepository.Create(order);
                return _mapper.Map<OrderLocationDTO>(nOrder);
            }
            catch (Exception e)
            {
                _motoService.UpdateMoto(order.VehicleId, false);
                throw new PersonalizeExceptions("Errors occurred while saving the record, please try again later");
            }
        }

        public async Task<OrderLocationDTO> Update(OrderLocationDTO orderLocationDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderLocationDTO> Cancel(OrderLocationCancelDTO cancelOrderDTO)
        {
            if (cancelOrderDTO.DatePreview >= DateTime.Now) throw new PersonalizeExceptions("The expected end date must be greater than or equal to the current date");

            var order = await _orderRepository.Get(cancelOrderDTO.Id);
            if (order == null) throw new PersonalizeExceptions("Rental Request Id is not existing, please check and try again later");

            order.CalculateFineRates(cancelOrderDTO.DatePreview);

            var availableAgain = _motoService.UpdateMoto(order.VehicleId, false);

            if (!availableAgain) throw new PersonalizeExceptions("Failed to update the vehicle, try again later");

            var uOrder = await _orderRepository.Update(order);

            return _mapper.Map<OrderLocationDTO>(uOrder);
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

        #region Private

        private bool CnhTypeValidation(string cnhType)
        {
            var list = new List<string> { "A", "AB" };
            return list.Contains(cnhType);
        }

        #endregion
    }
}
