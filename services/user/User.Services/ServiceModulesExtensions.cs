
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using User.Infra.Interfaces;
using User.Infra.Repositories;
using User.Services.Interfaces;
using User.Services.Service;

namespace User.Services
{
    public static class ServiceModulesExtensions
    {
        public static IServiceCollection AddServicesModules(this IServiceCollection services)
        {
            services.AddServiceScoped();
            services.AddServiceRedis();
            return services;
        }

        private static IServiceCollection AddServiceScoped(this IServiceCollection services)
        {
            services.AddScoped<IClientServices, ClientServices>();
            services.AddScoped<IUserRepository, UserRepository>();


            services.AddScoped<IClientImageUploadService, ClientImageUploadService>();


            return services;
        }

        private static IServiceCollection AddServiceRedis(this IServiceCollection services)
        {
            services.AddScoped<IRedisService, RedisService>(p =>
            {
                var configuration = p.GetRequiredService<IConfiguration>();
                var host = configuration["Redis:HostName"];
                var port = Convert.ToInt32(configuration["Redis:Port"]);
                // Adiciona um log para verificar a configuração
                Console.WriteLine($"Redis HostName: {host}");
                Console.WriteLine($"Redis Port: {port}");
                return new RedisService(host!, port!);
            });
            return services;
        }

    }
}
