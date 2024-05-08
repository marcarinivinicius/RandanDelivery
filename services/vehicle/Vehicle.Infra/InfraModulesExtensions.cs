using Microsoft.Extensions.DependencyInjection;
using Vehicle.Infra.Context;
using Vehicle.Infra.Interfaces;
using Vehicle.Infra.Repositories;

namespace Vehicle.Infra
{
    public static class InfraModulesExtensions
    {
        public static IServiceCollection AddInfraModules(this IServiceCollection services)
        {
            services.AddDatabaseContext();
            services.AddServiceMessage();
            return services;
        }
        private static IServiceCollection AddDatabaseContext(this IServiceCollection services)
        {
            services.AddScoped<IMotoRepository, MotoRepository>();
            services.AddDbContextFactory<VehicleContext>();
            return services;
        }

        private static IServiceCollection AddServiceMessage(this IServiceCollection services)
        {
            services.AddScoped<ProducerEvent>();
            services.AddSingleton<MbClient>();
            services.AddSingleton<MotorcycleRpcServer>();
            return services;
        }
    }
}
