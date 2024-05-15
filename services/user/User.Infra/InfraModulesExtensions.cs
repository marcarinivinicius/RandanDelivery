using AWS.Notify.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMq.Notify.Interfaces;
using RabbitMq.Notify.Services;
using RabbitMq.Notify.Services.RabbitMq.Notify.Services;
using RabbitMQ.Client;
using User.Infra.Context;
using User.Infra.Interfaces;
using User.Infra.Messages;
using User.Infra.Repositories;

namespace User.Infra
{
    public static class InfraModulesExtensions
    {
        public static IServiceCollection AddInfraModules(this IServiceCollection services)
        {
            services.AddDatabase();
            services.AddServiceMessageRabbit();
            services.AddServiceMessageSqs();
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddDbContextFactory<UserContext>();

            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }

        public static IServiceCollection AddServiceMessageRabbit(this IServiceCollection services)
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
            services.AddSingleton<UserRpcListener>();
            return services;
        }

        public static IServiceCollection AddServiceMessageSqs(this IServiceCollection services)
        {
            services.AddSingleton<ISqsConnection>();

            return services;
        }

    }
}
