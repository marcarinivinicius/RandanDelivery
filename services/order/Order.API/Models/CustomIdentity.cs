using Order.Services.EnumsDTO;
using System.Security.Claims;

namespace Order.API.Models
{
    public class CustomIdentity : ClaimsIdentity
    {
        public EnumRoleUserDTO CustomRole { get; set; }

        public CustomIdentity(string email, string name, EnumRoleUserDTO role) : base("custom")
        {
            // Adicione as reivindicações padrão, como o nome de usuário
            AddClaim(new Claim(ClaimTypes.Email, email));
            AddClaim(new Claim(ClaimTypes.Name, name));

            CustomRole = role;
        }
    }
}
