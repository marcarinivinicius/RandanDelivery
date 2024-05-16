using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMq.Notify.Interfaces;
using RabbitMq.Notify.Services;
using RabbitMQ.Client;
using RabbitMq.Notify.Services.RabbitMq.Notify.Services;
using Vehicle.Infra.Context;
using Vehicle.Infra.Interfaces;
using Vehicle.Infra.Messages;
using Vehicle.Infra.Repositories;
using AWS.Notify.Interfaces;
using Amazon.Runtime;
using AWS.Notify.Services;
using AWS.Notify.Utils;

namespace Vehicle.Infra
{
    public static class InfraModulesExtensions
    {
        public static IServiceCollection AddInfraModules(this IServiceCollection services)
        {
            services.AddDatabaseContext();
            services.AddServiceMessageRabbit();
            services.AddServiceMessageSqs();
            return services;
        }
        private static IServiceCollection AddDatabaseContext(this IServiceCollection services)
        {
            services.AddScoped<IMotoRepository, MotoRepository>();

            services.AddDbContextFactory<VehicleContext>();
            //(serviceProvider, optionsBuilder) =>
            //{
            //    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            //    optionsBuilder.UseNpgsql(configuration["Postgres:ConnectionString"]);
            //}
            return services;
        }

        private static IServiceCollection AddServiceMessageRabbit(this IServiceCollection services)
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

        public static IServiceCollection AddServiceMessageSqs(this IServiceCollection services)
        {

            services.AddSingleton<ISqsConnection>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var accessKey = "";
                var secretKey = "";
                var region = "";
                var urlQueue = "";

                if (!string.IsNullOrWhiteSpace(configuration["AwsNotify:accessKey"]))
                    accessKey = configuration["AwsNotify:accessKey"];
                if (!string.IsNullOrWhiteSpace(configuration["AwsNotify:secretKey"]))
                    secretKey = configuration["AwsNotify:secretKey"];
                if (!string.IsNullOrWhiteSpace(configuration["AwsNotify:region"]))
                    region = configuration["AwsNotify:region"];
                if (!string.IsNullOrWhiteSpace(configuration["AwsNotify:urlQueue"]))
                {
                    urlQueue = configuration["AwsNotify:urlQueue"];

                    if (urlQueue!.EndsWith("/"))
                    {
                        urlQueue = urlQueue.Substring(0, urlQueue.Length - 1);
                    }
                }

                var credentials = new BasicAWSCredentials(accessKey, secretKey);

                return new SqsConnection(credentials, region!, urlQueue);
            });
            services.AddSingleton<SendSQS>();
            return services;
        }
    }
}
