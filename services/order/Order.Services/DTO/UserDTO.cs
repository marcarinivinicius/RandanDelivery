using Order.Services.EnumsDTO;

namespace Order.Services.DTO
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CPFCnpj { get; set; }
        public DateTime Birth { get; set; }
        public string CnhNumber { get; set; }
        public string CnhType { get; set; }
        public string CnhImage { get; set; }
        public EnumRoleUserDTO Role { get; set; }
    }
}
