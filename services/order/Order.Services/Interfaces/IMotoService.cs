
using Order.Services.DTO;

namespace Order.Services.Interfaces
{
    public interface IMotoService
    {
        public Task<MotoDTO> GetMotoAvailable();
        public bool UpdateMoto(long id, bool located);
    }
}
