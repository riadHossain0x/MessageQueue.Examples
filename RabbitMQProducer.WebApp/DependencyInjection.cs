using RabbitMQ.Client;
using RabbitMQProducer.WebApp.Services;

namespace RabbitMQProducer.WebApp;

public static class DependencyInjection
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionProvider>(new ConnectionProvider("amqp://guest:guest@localhost:5672"));
        
        // Implement factory pattern
        services.AddScoped<IPublisher>(x => new Publisher(x.GetService<IConnectionProvider>()!,
                                                          "report_exchange",
                                                          ExchangeType.Topic));
        return services;
    }
}
