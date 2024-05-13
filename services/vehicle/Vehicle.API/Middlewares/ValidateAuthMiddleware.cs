using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Vehicle.API.Models;
using Vehicle.Services.Interfaces;

namespace Vehicle.API.Middlewares
{
    public class ValidateAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ValidateAuthMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]!);

                try
                {
                    // Valida o token JWT
                    var tokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

                    if (validatedToken is JwtSecurityToken jwtSecurityToken && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var email = principal.Identity!.Name;
                        var user = userService.GetLoggedInUser(email!);
                        if (user != null)
                        {
                            // Crie a CustomIdentity com base nos dados obtidos do serviço externo
                            var customIdentity = new CustomIdentity(user.Email, user.Name, user.Role);

                            // Defina a identidade personalizada no contexto HTTP
                            context.User = new ClaimsPrincipal(customIdentity);

                            await _next(context);
                            return;
                        }
                    }
                }
                catch
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }
            }

            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
