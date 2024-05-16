
using Microsoft.Extensions.DependencyInjection;
using Vehicle.Infra.Interfaces;
using Vehicle.Infra.Repositories;
using Vehicle.Services.Interfaces;
using Vehicle.Services.Profiles;
using Vehicle.Services.Service;

namespace Vehicle.Services
{
    public static class ServiceModulesExtensions
    {
        public static IServiceCollection AddServicesModules(this IServiceCollection services)
        {
            services.AddServiceScoped();
            services.AddMappersServices();
            return services;
        }

        private static IServiceCollection AddServiceScoped(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<IMotoRepository, MotoRepository>();
            services.AddScoped<IMotoService, MotoService>();

            return services;
        }

        private static IServiceCollection AddMappersServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MotoProfile));
            return services;
        }
    }
}
