
using Order.Services.Models;

namespace Order.Services.Interfaces
{
    public interface IUserService
    {
        public Task<UserModel> GetLoggedInUser(string email);
    }
}
