using User.Domain.Entities;

namespace User.Services.Interfaces
{
    public interface IClientServices
    {
        Task<Client> Create(Client client);
        Task<Client> Update(Client client);
        Task Remove(long id);
        Task<Client> Get(long id);
        Task<Client> GetByEmail(string email);
        Task<List<Client>> GetAll();
    }
}
