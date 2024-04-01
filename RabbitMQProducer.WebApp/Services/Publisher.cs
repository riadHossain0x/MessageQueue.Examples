using RabbitMQ.Client;
using System.Text;

namespace RabbitMQProducer.WebApp.Services;

public interface IPublisher : IDisposable
{
    void Publish(string message, string routingKey, IDictionary<string, object>? messageAttribute, string? timeToLive = "30000");
    // Implement publishing message with queue 
}
public class Publisher : IPublisher
{
    private readonly IConnectionProvider _connectionProvider;
    private readonly string _exchangeName;
    private readonly IModel _channel;
    private bool _disposed;

    public Publisher(IConnectionProvider connectionProvider, string exchangeName, string exchangeType, int timeToLive = 30)
    {
        _connectionProvider = connectionProvider;
        _exchangeName = exchangeName;
        _channel = _connectionProvider.GetConnection().CreateModel();

        // in some cases time to live will be null, implement this also
        var ttl = new Dictionary<string, object>
        {
            {"x-message-ttl", timeToLive}
        };
        _channel.ExchangeDeclare(_exchangeName, exchangeType, false, false, arguments: ttl);
    }

    public void Publish(string message, string routingKey, IDictionary<string, object>? messageAttribute, string? timeToLive = "30000")
    {
        var body = Encoding.UTF8.GetBytes(message);
        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.Headers = messageAttribute;
        properties.Expiration = timeToLive;

        _channel.BasicPublish(_exchangeName, routingKey, properties, body);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Protected implementation of Dispose pattern.
    protected virtual void Dispose(bool disposing) 
    {
        if (_disposed) return;

        if(disposing) _channel?.Dispose();

        _disposed = true;
    }
}
