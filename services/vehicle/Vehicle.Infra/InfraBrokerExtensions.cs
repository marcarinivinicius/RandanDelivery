using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vehicle.Infra.Messages;


namespace Vehicle.Infra
{
    public static class InfraBrokerExtensions
    {
        private static IServiceProvider _serviceProvider;

        public static void UseRabbitListener(this IApplicationBuilder app)
        {
            _serviceProvider = app.ApplicationServices;

            var lifetime = _serviceProvider.GetService<IHostApplicationLifetime>();
            lifetime?.ApplicationStarted.Register(OnApplicationStartedRabbit);
            lifetime?.ApplicationStopping.Register(OnApplicationStoppingRabbit);
        }

        private static void OnApplicationStartedRabbit()
        {
            // Inicia o consumo das filas ao iniciar a aplicação
            var userConsumer = _serviceProvider.GetService<MotoConsumer>();
            userConsumer?.Consume("direct");

            var userRpcListener = _serviceProvider.GetService<MotoRpcListener>();
            userRpcListener?.Consume("publishMoto");
        }

        private static void OnApplicationStoppingRabbit()
        {
            // Executa ações ao parar a aplicação, se necessário
        }


        public static void UseAWSListener(this IApplicationBuilder app)
        {
            _serviceProvider = app.ApplicationServices;

            var lifetime = _serviceProvider.GetService<IHostApplicationLifetime>();
            lifetime?.ApplicationStarted.Register(OnApplicationStartedAWS);
            lifetime?.ApplicationStopping.Register(OnApplicationStoppingAWS);
        }

        private static void OnApplicationStartedAWS()
        {
            var usersqsListener = _serviceProvider.GetService<SqsConsumer>();
            usersqsListener?.Consume("fyMoto");
        }

        private static void OnApplicationStoppingAWS()
        {
            // Executa ações ao parar a aplicação, se necessário
        }
    }
}
