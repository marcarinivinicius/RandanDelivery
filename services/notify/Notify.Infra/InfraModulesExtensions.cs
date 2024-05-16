using Amazon.Runtime;
using AWS.Notify.Interfaces;
using AWS.Notify.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notify.Infra.Context;
using Notify.Infra.Interfaces;
using Notify.Infra.Repositories;
using Vehicle.Infra.Messages;

namespace Notify.Infra
{
    public static class InfraModulesExtensions
    {
        public static IServiceCollection AddInfraModules(this IServiceCollection services)
        {
            services.AddDatabaseContext();
            services.AddServiceMessageSqs();
            return services;
        }
        private static IServiceCollection AddDatabaseContext(this IServiceCollection services)
        {
            services.AddScoped<INotificationRepository, NotificationRepository>();

            services.AddDbContextFactory<NotificationContext>();
            //(serviceProvider, optionsBuilder) =>
            //{
            //    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            //    optionsBuilder.UseNpgsql(configuration["Postgres:ConnectionString"]);
            //}
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

                return new SqsConnection(credentials, region, urlQueue);
            });
            services.AddSingleton<SqsConsumer>();

            return services;
        }
    }
}
