using Newtonsoft.Json;
using Order.Services.Interfaces;
using Order.Services.Models;
using RabbitMq.Notify.DataModels;
using RabbitMq.Notify.Services;

namespace Order.Services.Services
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
