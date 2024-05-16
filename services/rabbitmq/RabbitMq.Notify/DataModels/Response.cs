namespace RabbitMq.Notify.DataModels
{
    public class Response
    {
        public bool Success { get; set; }
        public string ErrMessage { get; set; }
        public dynamic Payload { get; set; }
    }
}
