using User.Infra.Interfaces;
using Vehicle.Domain.Entities;
using Vehicle.Infra.Models;

namespace Vehicle.Infra.Interfaces
{
    public interface IMotoRepository : IBaseRepository<Moto>
    {
        Task<List<Moto>> GetAll(MotoFilters? filters = null);
    }
}
