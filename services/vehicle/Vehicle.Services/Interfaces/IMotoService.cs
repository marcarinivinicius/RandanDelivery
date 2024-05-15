using Vehicle.Infra.Models;
using Vehicle.Services.DTO;

namespace Vehicle.Services.Interfaces
{
    public interface IMotoService
    {
        Task<MotoDTO> Create(MotoDTO client);
        Task<MotoDTO> Update(MotoDTO client);
        Task Remove(long id);
        Task<MotoDTO> Get(long id);
        Task<List<MotoDTO>> GetAll(MotoFilters filters);

    }
}
