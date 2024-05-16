using User.Domain.Entities;

namespace User.Infra.Interfaces
{
    public interface IUserRepository : IBaseRepository<Client>
    {
        Task<Client> GetByEmail(string email);
        Task<Client> GetByCPFCNPJ(string cpfcpnj);
        Task<List<Client>> GetAll();
        Task<Client> GetByCnh(string cnh);

    }
}
