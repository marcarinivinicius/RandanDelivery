﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using User.Infra.Messages;
using User.Infra.Messages.Consumers;

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
            var userConsumer = _serviceProvider.GetService<UserConsumer>();
            userConsumer?.Consume("direct");

            var userRpcListener = _serviceProvider.GetService<UserRpcListener>();
            userRpcListener?.Consume("publishUser");

            // SQS


        }

        private static void OnApplicationStopping()
        {
            // Executa ações ao parar a aplicação, se necessário
        }
    }
}
