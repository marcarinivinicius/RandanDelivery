namespace Order.Services.Models
{
    public class UserModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public EnumRole Role { get; set; }
    }
    public enum EnumRole
    {
        User,
        Admin
    }
}
