using Newtonsoft.Json;
using RabbitMq.Notify.DataModels;
using RabbitMq.Notify.Services;
using Vehicle.Services.Interfaces;
using Vehicle.Services.Model;

namespace Vehicle.Services.Service
{
    public class UserService : IUserService
    {
        private readonly RabbitMqClient _rabbitMqClient;
        public UserService(RabbitMqClient rabbitMqClient)
        {
            _rabbitMqClient = rabbitMqClient;
        }

        public async Task<UserModel> GetLoggedInUser(string email)
        {
            var userpayload = _rabbitMqClient.Call(new Request
            {
                Method = "GetUserLogged",
                Payload = new { Email = email }
            });
            var userlogged = JsonConvert.DeserializeObject<UserModel>(JsonConvert.SerializeObject(userpayload!.Payload));

            return userlogged;


        }
    }
}
