using AWS.Notify.DataModels;
using AWS.Notify.Utils;
using Newtonsoft.Json;
using RabbitMq.Notify.DataModels;
using System.Text.Json.Nodes;
using Vehicle.Services.DTO;
using Vehicle.Services.Interfaces;

namespace Vehicle.Services.Service
{
    public class NotificationService : INotificationService
    {
        private readonly SendSQS _sendSQS;

        public NotificationService(SendSQS sendSQS)
        {
            _sendSQS = sendSQS;
        }

        public async Task SendNewMoto(MotoDTO moto)
        {
            var data = new RequestDataAws
            {
                Message = "This vehicle was manufactured in 2024",
                MetaData = moto
            };
            var message = JsonConvert.SerializeObject(data);

            await _sendSQS.Send(
                new RequestAws
                {
                    Method = "fyFabrication",
                    Payload = message
                }, "notify");
        }
    }
}
