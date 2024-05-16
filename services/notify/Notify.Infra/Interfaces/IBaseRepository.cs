using Notify.Domain.Entities;

namespace Notify.Infra.Interfaces
{
    public interface IBaseRepository<T> where T : ModelBase
    {
        Task<T> Get(long id);
        Task<T> Create(T obj);
        Task<T> Update(T obj);
        Task Delete(long id);
    }
}
