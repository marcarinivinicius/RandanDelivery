
using Order.Services.DTO;

namespace Order.Services.Interfaces
{
    public interface IUserService
    {
        public Task<UserDTO> GetLoggedInUser(string email);
        public Task<UserDTO> GetUser(long id);
    }
}
