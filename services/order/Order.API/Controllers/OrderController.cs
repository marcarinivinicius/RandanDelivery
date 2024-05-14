using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Order.API.Models;
using Order.API.ViewModels.Order;
using Order.Domain.Exceptions;
using Order.Services.DTO;
using Order.Services.Interfaces;
using Vehicle.API.ViewModels;

namespace Order.API.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;


        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateOrderViewModel createOrderViewModel)
        {
            CustomIdentity? customIdentity = User.Identity as CustomIdentity;

            try
            {
                var order = _mapper.Map<OrderLocationDTO>(createOrderViewModel);
                order.UserId = customIdentity!.UserId;

                var newOrder = await _orderService.Create(order);
                return Ok(new ResultModel
                {
                    Success = true,
                    Message = "Order added successfully",
                    MetaData = newOrder
                });
            }
            catch (PersonalizeExceptions ex)
            {
                return BadRequest(GenericResponse.DomainError(ex.Message, ex.Err!));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GenericResponse.GenericApplicationError(ex.Message));
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UpdateOrderViewModel updateOrderViewModel)
        {

            try
            {
                var orderDTO = _mapper.Map<OrderLocationDTO>(updateOrderViewModel);
                var order = await _orderService.Update(orderDTO);
                return Ok(new ResultModel
                {
                    Success = true,
                    Message = "Order updated successfully",
                    MetaData = order
                });
            }
            catch (PersonalizeExceptions ex)
            {
                return BadRequest(GenericResponse.DomainError(ex.Message, ex.Err!));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GenericResponse.GenericApplicationError(ex.Message));
            }
        }


        [HttpPut]
        [Route("cancel")]
        public async Task<IActionResult> Cancel([FromBody] CancelOrderViewModel motoViewModel)
        {
            CustomIdentity? customIdentity = User.Identity as CustomIdentity;

            try
            {
                var orderDTO = _mapper.Map<OrderLocationCancelDTO>(motoViewModel);
                var order = await _orderService.Cancel(orderDTO);
                return Ok(new ResultModel
                {
                    Success = true,
                    Message = "Order cancelled successfully",
                    MetaData = order
                });
            }
            catch (PersonalizeExceptions ex)
            {
                return BadRequest(GenericResponse.DomainError(ex.Message, ex.Err!));
            }
            catch (Exception ex)
            {
                return StatusCode(500, GenericResponse.GenericApplicationError(ex.Message));
            }
        }
    }
}
