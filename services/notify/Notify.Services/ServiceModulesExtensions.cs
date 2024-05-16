
using Microsoft.Extensions.DependencyInjection;
using Notify.Infra.Interfaces;
using Notify.Infra.Repositories;
using Notify.Services.Interfaces;
using Notify.Services.Profiles;
using Notify.Services.Service;

namespace Notify.Services
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
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            return services;
        }

        private static IServiceCollection AddMappersServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(NotificationProfile));
            return services;
        }
    }
}
