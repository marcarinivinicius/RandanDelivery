using AWS.Notify.DataModels;
using AWS.Notify.Utils;
using Newtonsoft.Json;
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
            try
            {
                await _sendSQS.Send(
                    new RequestAws
                    {
                        Method = "fyFabrication",
                        Payload = message
                    }, "notify");
            }
            catch (Exception ex)
            {
                //Não Gera exceção externa para não travar o fluxo, se não salvou a notificação tratar de outra forma. 
            }
        }
    }
}
