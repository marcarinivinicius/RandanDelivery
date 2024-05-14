using RabbitMq.Notify.Interfaces;

namespace RabbitMq.Notify.Services
{
    using System;
    using System.Net.Sockets;
    using Polly;
    using Polly.Retry;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using RabbitMQ.Client.Exceptions;

    namespace RabbitMq.Notify.Services
    {
        public class RabbitConnection : IRabbitConnection
        {
            private readonly IConnectionFactory _connectionFactory;
            private IConnection? _connection;
            private readonly int _maxRetryAttempts;
            private bool _disposed;

            public RabbitConnection(IConnectionFactory connectionFactory, int maxRetryAttempts)
            {
                _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
                _maxRetryAttempts = maxRetryAttempts;
            }

            public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

            public bool TryConnect()
            {
                var retryPolicy = RetryPolicy.Handle<SocketException>()
                                             .Or<BrokerUnreachableException>()
                                             .WaitAndRetry(_maxRetryAttempts, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

                retryPolicy.Execute(() =>
                {
                    _connection = _connectionFactory.CreateConnection();
                });

                if (IsConnected)
                {
                    RegisterConnectionEvents();
                    return true;
                }
                else
                {
                    // Logging: Falha ao conectar
                    return false;
                }
            }

            private void RegisterConnectionEvents()
            {
                _connection!.ConnectionBlocked += OnConnectionBlocked;
                _connection!.CallbackException += OnCallbackException;
                _connection!.ConnectionShutdown += OnConnectionShutdown;
            }

            private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
            {
                if (_disposed) return;
                // Logging: Conexão bloqueada
                TryReconnect();
            }

            private void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
            {
                if (_disposed) return;
                // Logging: Callback Exception
                TryReconnect();
            }

            private void OnConnectionShutdown(object? sender, ShutdownEventArgs reason)
            {
                if (_disposed) return;
                // Logging: Conexão encerrada
                TryReconnect();
            }

            private void TryReconnect()
            {
                if (_disposed) return;
                // Logging: Tentando reconectar
                TryConnect();
            }

            public IModel CreateChannel()
            {
                if (!IsConnected)
                {
                    // Logging: Nenhuma conexão disponível
                    throw new InvalidOperationException("No RabbitMQ connections are available to perform this action.");
                }

                return _connection!.CreateModel();
            }

            public void Dispose()
            {
                if (_disposed) return;

                _disposed = true;

                try
                {
                    _connection?.Dispose();
                }
                catch (Exception ex)
                {
                    // Logging: Falha ao descartar conexão
                }
            }
        }
    }

}
