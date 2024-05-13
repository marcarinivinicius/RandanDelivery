using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Vehicle.API.Models;
using Vehicle.API.ViewModels;
using Vehicle.API.ViewModels.Moto;
using Vehicle.Domain.Exceptions;
using Vehicle.Infra.Models;
using Vehicle.Services.DTO;
using Vehicle.Services.Interfaces;
using Vehicle.Services.Model;

namespace Vehicle.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MotoController : Controller
    {
        private readonly IMotoService _motoService;
        private readonly IMapper _mapper;

        public MotoController(IMotoService motoService, IMapper mapper)
        {
            _motoService = motoService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateMotoViewModel motoViewModel)
        {
            CustomIdentity? customIdentity = User.Identity as CustomIdentity;

            if (customIdentity!.CustomRole != EnumRole.Admin)
            {
                return Unauthorized(new ResultModel
                {
                    Success = false,
                    Message = "Login not authorized for this action",
                    MetaData = { }
                });
            }

            try
            {
                var vehicle = _mapper.Map<MotoDTO>(motoViewModel);
                var newVehicle = await _motoService.Create(vehicle);
                return Ok(new ResultModel
                {
                    Success = true,
                    Message = "Motorcycle added successfully",
                    MetaData = newVehicle
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
        public async Task<IActionResult> Update([FromBody] UpdateMotoViewModel motoViewModel)
        {
            CustomIdentity? customIdentity = User.Identity as CustomIdentity;

            if (customIdentity!.CustomRole != EnumRole.Admin)
            {
                return Unauthorized(new ResultModel
                {
                    Success = false,
                    Message = "Login not authorized for this action",
                    MetaData = { }
                });
            }

            try
            {
                var vehicle = _mapper.Map<MotoDTO>(motoViewModel);
                var motorcycle = await _motoService.Update(vehicle);
                return Ok(new ResultModel
                {
                    Success = true,
                    Message = "Motorcycle updated successfully",
                    MetaData = motorcycle
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

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            CustomIdentity? customIdentity = User.Identity as CustomIdentity;

            if (customIdentity!.CustomRole != EnumRole.Admin)
            {
                return Unauthorized(new ResultModel
                {
                    Success = false,
                    Message = "Login not authorized for this action",
                    MetaData = { }
                });
            }


            try
            {
                await _motoService.Remove(id);
                return Ok(new ResultModel
                {
                    Success = true,
                    Message = "Motorcycle deleted successfully",
                    MetaData = { }
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

        [HttpGet]
        [Route("{id}/")]
        public async Task<IActionResult> Get(long id)
        {
            CustomIdentity? customIdentity = User.Identity as CustomIdentity;

            if (customIdentity!.CustomRole != EnumRole.Admin)
            {
                return Unauthorized(new ResultModel
                {
                    Success = false,
                    Message = "Login not authorized for this action",
                    MetaData = { }
                });
            }

            try
            {
                var motorcycleDto = await _motoService.Get(id);
                if (motorcycleDto == null)
                {
                    return Ok(new ResultModel
                    {
                        Success = true,
                        Message = "Motorcycle not found",
                        MetaData = { }
                    });
                }
                return Ok(new ResultModel
                {
                    Success = true,
                    Message = "Motorcycle returned successfully",
                    MetaData = motorcycleDto
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

        [HttpGet]
        [Route("plate/{plateCode}")]
        public async Task<IActionResult> GetPlate(string plateCode)
        {
            CustomIdentity? customIdentity = User.Identity as CustomIdentity;

            if (customIdentity!.CustomRole != EnumRole.Admin)
            {
                return Unauthorized(new ResultModel
                {
                    Success = false,
                    Message = "Login not authorized for this action",
                    MetaData = { }
                });
            }

            try
            {
                MotoFilters motoFilters = new MotoFilters()
                {
                    PlateCode = plateCode,
                    AllRecords = true
                };

                var motorcycleDto = await _motoService.GetAll(motoFilters);
                if (motorcycleDto == null)
                {
                    return Ok(new ResultModel
                    {
                        Success = true,
                        Message = "Motorcycle not found",
                        MetaData = { }
                    });
                }
                return Ok(new ResultModel
                {
                    Success = true,
                    Message = "Motorcycle returned successfully",
                    MetaData = motorcycleDto
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
