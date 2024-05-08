
using Microsoft.Extensions.DependencyInjection;
using Vehicle.Infra.Interfaces;
using Vehicle.Infra.Repositories;

namespace Vehicle.Services
{
    public static class ServiceModulesExtensions
    {
        public static IServiceCollection AddServicesModules(this IServiceCollection services)
        {
            services.AddServiceScoped();
            return services;
        }

        private static IServiceCollection AddServiceScoped(this IServiceCollection services)
        {
            //services.AddScoped<IClientServices, ClientServices>();
            services.AddScoped<IMotoRepository, MotoRepository>();

            return services;
        }
    }
}
