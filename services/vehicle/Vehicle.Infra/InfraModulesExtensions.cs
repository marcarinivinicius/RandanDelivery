using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMq.Notify.Interfaces;
using RabbitMq.Notify.Services;
using RabbitMQ.Client;
using RabbitMq.Notify.Services.RabbitMq.Notify.Services;
using System;
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

            services.AddDbContextFactory<VehicleContext>((serviceProvider, optionsBuilder) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                optionsBuilder.UseNpgsql(configuration["Postgres:ConnectionString"]);
            });
            return services;
        }

        private static IServiceCollection AddServiceMessage(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitConnection>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();

                var factory = new ConnectionFactory()
                {
                    HostName = configuration["Rabbitmq:HostName"]
                };
                factory.AutomaticRecoveryEnabled = true;
                factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);
                factory.TopologyRecoveryEnabled = true;

                if (!string.IsNullOrWhiteSpace(configuration["Rabbitmq:UserName"]))
                    factory.UserName = configuration["Rabbitmq:UserName"];

                if (!string.IsNullOrWhiteSpace(configuration["Rabbitmq:Password"]))
                    factory.Password = configuration["Rabbitmq:Password"];
                var retryCount = 10;

                if (!string.IsNullOrWhiteSpace(configuration["Rabbitmq:RetryCount"]))
                    retryCount = int.Parse(configuration["Rabbitmq:RetryCount"]!);

                return new RabbitConnection(factory, retryCount);
            });

            services.AddScoped<RabbitProducer>();
            services.AddScoped(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
            services.AddSingleton<RabbitMqClient>();
            services.AddSingleton<MotoRpcListener>();
            return services;
        }
    }
}
