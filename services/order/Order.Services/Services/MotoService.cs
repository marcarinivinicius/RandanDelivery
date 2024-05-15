using Newtonsoft.Json;
using Order.Infra.Models;
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

        public MotoDTO GetMotoAvailable()
        {
            var filterMoto = new MotoFilters()
            {
                AllLocated = false,
                Located = false,
                AllRecords = false,
                Active = true,
            };

            var payload = _rabbitMqClient.Call(new Request
            {
                Method = "GetMotoFilter",
                Payload = new { Filters = filterMoto }
            }, "publishMoto");

            if (payload!.Payload == null)
            {
                return null;
            }
            var userlogged = JsonConvert.DeserializeObject<MotoDTO>(JsonConvert.SerializeObject(payload!.Payload));

            return userlogged;
        }

        public bool UpdateMoto(long id, bool isLocated)
        {
            var response = _rabbitMqClient.Call(new Request
            {
                Method = "UpdateMoto",
                Payload = new { Id = id, Located = isLocated }
            }, "publishMoto");
            var isUpdated = (bool)JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(response!.Payload)).IsUpdate;

            return isUpdated;
        }
    }
}
