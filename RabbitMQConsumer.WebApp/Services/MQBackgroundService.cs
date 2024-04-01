
namespace RabbitMQConsumer.WebApp.Services;

public class MQBackgroundService : IHostedService
{
    private readonly ISubscriber _subscriber;

    public MQBackgroundService(ISubscriber subscriber)
    {
        _subscriber = subscriber;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _subscriber.Subscribe(ProcessMessage);
        return Task.CompletedTask;
    }

    private bool ProcessMessage(string message, IDictionary<string, object> headers)
    {
        Console.WriteLine(message);
        return true;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
