

namespace Vehicle.Services.Model
{
    public class UserModel
    {
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
