using Newtonsoft.Json;
using Order.Services.DTO;
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

        public Task<UserDTO> GetLoggedInUser(string email)
        {
            var userpayload = _rabbitMqClient.Call(new Request
            {
                Method = "GetUserLogged",
                Payload = new { Email = email }
            });
            var userlogged = JsonConvert.DeserializeObject<UserDTO>(JsonConvert.SerializeObject(userpayload!.Payload));

            return userlogged;
        }

        public Task<UserDTO> GetUser(long id)
        {
            var userpayload = _rabbitMqClient.Call(new Request
            {
                Method = "GetUser",
                Payload = new { Id = id }
            });
            var userlogged = JsonConvert.DeserializeObject<UserDTO>(JsonConvert.SerializeObject(userpayload!.Payload));

            return userlogged;
        }


    }
}
