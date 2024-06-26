﻿using System.Security.Claims;
using Vehicle.Services.Model;

namespace Vehicle.API.Models
{
    public class CustomIdentity : ClaimsIdentity
    {
        public EnumRole CustomRole { get; set; }

        public CustomIdentity(string email, string name, EnumRole role) : base("custom")
        {
            // Adicione as reivindicações padrão, como o nome de usuário
            AddClaim(new Claim(ClaimTypes.Email, email));
            AddClaim(new Claim(ClaimTypes.Name, name));

            CustomRole = role;
        }
    }
}
