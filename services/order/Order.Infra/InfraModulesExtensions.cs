using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Infra.Context;
using Order.Infra.Interfaces;
using Order.Infra.Messages;
using Order.Infra.Repositories;
using RabbitMq.Notify.Interfaces;
using RabbitMq.Notify.Services;
using RabbitMq.Notify.Services.RabbitMq.Notify.Services;
using RabbitMQ.Client;

namespace Order.Infra
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
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddDbContextFactory<OrderContext>();
            //(serviceProvider, optionsBuilder) =>
            //{
            //    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            //    optionsBuilder.UseNpgsql(configuration["Postgres:ConnectionString"]);
            //}
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
            services.AddSingleton<OrderRpcListener>();
            return services;
        }
    }
}
