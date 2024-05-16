using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vehicle.Infra.Messages;


namespace Notify.Infra
{
    public static class InfraBrokerExtensions
    {
        private static IServiceProvider _serviceProvider;

        public static void UseAWSListener(this IApplicationBuilder app)
        {
            _serviceProvider = app.ApplicationServices;

            var lifetime = _serviceProvider.GetService<IHostApplicationLifetime>();
            lifetime?.ApplicationStarted.Register(OnApplicationStartedAWS);
            lifetime?.ApplicationStopping.Register(OnApplicationStoppingAWS);
        }

        private static void OnApplicationStartedAWS()
        {
            var cancellationToken = _serviceProvider.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping;

            var usersqsListener = _serviceProvider.GetService<SqsConsumer>();
            usersqsListener?.Consume("notify", cancellationToken);
        }

        private static void OnApplicationStoppingAWS()
        {
            // Executa ações ao parar a aplicação, se necessário
        }
    }
}
