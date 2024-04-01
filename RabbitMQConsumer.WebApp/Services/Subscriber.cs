using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQConsumer.WebApp.Services;

public interface ISubscriber : IDisposable
{
    void Subscribe(Func<string, IDictionary<string, object>, bool> callback);
    void SubscribeAsync(Func<string, IDictionary<string, object>, Task<bool>> callback);
}
public class Subscriber : ISubscriber
{
    private readonly IConnectionProvider _connectionProvider;
    private readonly string _exchangeName;
    private readonly string _queueName;
    private readonly IModel _channel;
    private bool _multipleAck = false;
    private string _consumerTag = string.Empty;
    private bool _disposed = false;

    public Subscriber(IConnectionProvider connectionProvider,
                      string exchangeName,
                      string queueName,
                      string routingKey,
                      string exchangeType,
                      int timeToLive = 30000,
                      ushort prefetchSize = 0,
                      ushort prefetchCount = 1)
    {
        _connectionProvider = connectionProvider;
        _exchangeName = exchangeName;
        _queueName = queueName;
        _channel = _connectionProvider.GetConnection().CreateModel();
        var ttl = new Dictionary<string, object>
        {
            {"x-message-ttl", timeToLive }
        };
        _channel.ExchangeDeclare(_exchangeName, exchangeType, arguments: ttl);
        _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueBind(_queueName, _exchangeName, routingKey);
        _channel.BasicQos(prefetchSize, prefetchCount, false);

        if (prefetchCount > 1) _multipleAck = true;
    }

    public void Subscribe(Func<string, IDictionary<string, object>, bool> callback)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (sender, args) =>
        {
            var body = args.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            var success = callback.Invoke(message, args.BasicProperties.Headers);
            if (success)
            {
                _channel.BasicAck(args.DeliveryTag, _multipleAck);
            }
        };

        _consumerTag = _channel.BasicConsume(_queueName, false, consumer);
    }

    public void SubscribeAsync(Func<string, IDictionary<string, object>, Task<bool>> callback)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (sender, args) =>
        {
            var body = args.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            var success = await callback.Invoke(message, args.BasicProperties.Headers);
            if (success)
            {
                _channel.BasicAck(args.DeliveryTag, _multipleAck);
            }
        };

        _consumerTag = _channel.BasicConsume(_queueName, false, consumer);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if(_disposed) return;

        if(disposing)
        {
            _channel?.BasicCancel(_consumerTag);
            _channel?.Close();
        }

        _disposed = true;
    }
}
