
using Order.Services.DTO;

namespace Order.Services.Interfaces
{
    public interface IMotoService
    {
        public MotoDTO GetMotoAvailable();
        public bool UpdateMoto(long id, bool located);
    }
}
