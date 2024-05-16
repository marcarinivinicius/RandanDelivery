namespace Notify.Domain.Entities
{
    public sealed record Notification : ModelBase
    {
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }


        protected Notification() { }

        public Notification(string message)
        {
            Message = message;
            CreatedAt = DateTime.Now;
        }

    }
}
