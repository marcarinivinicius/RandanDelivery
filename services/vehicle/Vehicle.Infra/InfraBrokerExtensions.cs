using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vehicle.Infra.Messages;
using Vehicle.Infra.Messages.Consumers;


namespace User.Infra
{
    public static class InfraBrokerExtensions
    {
        private static IServiceProvider _serviceProvider;

        public static void UseRabbitListener(this IApplicationBuilder app)
        {
            _serviceProvider = app.ApplicationServices;

            var lifetime = _serviceProvider.GetService<IHostApplicationLifetime>();
            lifetime?.ApplicationStarted.Register(OnApplicationStarted);
            lifetime?.ApplicationStopping.Register(OnApplicationStopping);
        }

        private static void OnApplicationStarted()
        {
            // Inicia o consumo das filas ao iniciar a aplicação
            var userConsumer = _serviceProvider.GetService<MotoConsumer>();
            userConsumer?.Consume("direct");

            var userRpcListener = _serviceProvider.GetService<MotoRpcListener>();
            userRpcListener?.Consume("publish");
        }

        private static void OnApplicationStopping()
        {
            // Executa ações ao parar a aplicação, se necessário
        }
    }
}
