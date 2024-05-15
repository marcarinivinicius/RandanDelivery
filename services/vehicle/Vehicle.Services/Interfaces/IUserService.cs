using Vehicle.Services.Model;

namespace Vehicle.Services.Interfaces
{
    public interface IUserService
    {
        public Task<UserModel> GetLoggedInUser(string email);
    }
}
