
using Microsoft.Extensions.DependencyInjection;
using Order.Infra.Interfaces;
using Order.Infra.Repositories;
using Order.Services.Interfaces;
using Order.Services.Profiles;
using Order.Services.Services;

namespace Order.Services
{
    public static class ServiceModulesExtensions
    {
        public static IServiceCollection AddServicesModules(this IServiceCollection services)
        {
            services.AddServiceScoped();
            services.AddMappersServices();
            return services;
        }

        private static IServiceCollection AddServiceScoped(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            //services.AddScoped<IClientServices, ClientServices>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }

        private static IServiceCollection AddMappersServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(OrderProfile));
            return services;
        }
    }
}
