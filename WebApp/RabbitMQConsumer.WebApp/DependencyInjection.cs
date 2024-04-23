using RabbitMQ.Client;
using RabbitMQConsumer.WebApp.Services;

namespace RabbitMQConsumer.WebApp;

public static class DependencyInjection
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionProvider>(new ConnectionProvider("amqp://guest:guest@localhost:5672"));
        services.AddSingleton<ISubscriber>(x => new Subscriber(x.GetService<IConnectionProvider>()!,
                                                               "report_exchange",
                                                               "report_queue",
                                                               "report.*",
                                                               ExchangeType.Topic));
        services.AddHostedService<MQBackgroundService>();
        return services;
    }
}
