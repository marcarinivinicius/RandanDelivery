using Vehicle.Services.Model;

namespace Vehicle.Services.Interfaces
{
    public interface IUserService
    {
        public UserModel GetLoggedInUser(string email);
    }
}
