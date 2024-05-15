﻿using Newtonsoft.Json;
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

        public Task<MotoDTO> GetMotoAvailable()
        {
            var filterMoto = new MotoFilters()
            {
                Located = false,
                Active = true,
                AllRecords = false,
            };

            var payload = _rabbitMqClient.Call(new Request
            {
                Method = "GetMotoFilter",
                Payload = new { Filters = filterMoto }
            });
            if (payload!.Payload == null)
            {
                return null;
            }
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
