namespace Notify.Services.DTO
{
    public sealed record NotificationDTO
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }


        protected NotificationDTO() { }

        public NotificationDTO(string message, DateTime createdAt)
        {
            Message = message;
            CreatedAt = createdAt;
        }
    }
}
