using Newtonsoft.Json;
using Order.Services.DTO;
using Order.Services.Interfaces;
using RabbitMq.Notify.DataModels;
using RabbitMq.Notify.Services;

namespace Order.Services.Services
{
    public class MotoService : IMotoService
    {
        private readonly RabbitMqClient _rabbitMqClient;

        public MotoService(RabbitMqClient rabbitMqClient)
        {
            _rabbitMqClient = rabbitMqClient;
        }

        public Task<MotoDTO> GetMotoAvailable()
        {
            var payload = _rabbitMqClient.Call(new Request
            {
                Method = "GetMotoAvailable",
                Payload = { }
            });
            var userlogged = JsonConvert.DeserializeObject<UserDTO>(JsonConvert.SerializeObject(payload!.Payload));

            return userlogged;
        }

        public bool UpdateMoto(long id, bool isLocated)
        {
            var response = _rabbitMqClient.Call(new Request
            {
                Method = "UpdateMoto",
                Payload = new { Id = id, Located = isLocated }
            });
            var isUpdated = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(response!.Payload));

            return isUpdated;
        }
    }
}
