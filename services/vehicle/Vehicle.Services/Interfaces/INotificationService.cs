using Vehicle.Services.DTO;

namespace Vehicle.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendNewMoto(MotoDTO moto);
    }
}
