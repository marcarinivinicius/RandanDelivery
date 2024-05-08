using Microsoft.Extensions.DependencyInjection;
using RabbitMq.Notify.Services;
using Vehicle.Infra.Context;
using Vehicle.Infra.Interfaces;
using Vehicle.Infra.Messages;
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
            services.AddScoped<RabbitProducer>();
            services.AddSingleton<RabbitMqClient>();
            services.AddSingleton<MotoRpcListener>();
            return services;
        }
    }
}
