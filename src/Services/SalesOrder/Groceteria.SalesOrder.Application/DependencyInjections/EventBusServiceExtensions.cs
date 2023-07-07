using Groceteria.Infrastructure.EventBus.Message.Common.Constants;
using Groceteria.SalesOrder.Application.Events.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Groceteria.SalesOrder.Application.DependencyInjections
{
    public static class EventBusServiceExtensions
    {
        public static IServiceCollection AddEventBusServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(config =>
            {
                config.AddConsumer<BasketCheckoutConsumer>();
                config.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri(configuration["RabbitMq:ConnectionString"]));
                    configurator.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
                    {
                        c.ConfigureConsumer<BasketCheckoutConsumer>(context);
                        c.ExchangeType = ExchangeType.Topic;
                    });
                });
            });

            services.AddScoped<BasketCheckoutConsumer>();
            return services;
        }
    }
}
