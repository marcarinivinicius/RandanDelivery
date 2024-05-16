using Microsoft.AspNetCore.Mvc;
using Notify.API.ViewModels;
using Notify.Domain.Exceptions;
using Notify.Services.Interfaces;

namespace Notify.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [Route("/GetAll")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var notificationsDTO = await _notificationService.GetAll();
                if (notificationsDTO == null)
                {
                    return Ok(new ResultModel
                    {
                        Success = true,
                        Message = "No registered notifications found",
                        MetaData = { }
                    });
                }
                return Ok(new ResultModel
                {
                    Success = true,
                    Message = "Notifications returned successfully",
                    MetaData = notificationsDTO
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
