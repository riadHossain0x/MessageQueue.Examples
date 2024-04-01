using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;

namespace RabbitMQProducer.WebApp.Services;

public interface IConnectionProvider : IDisposable
{
    IConnection GetConnection();
}

public class ConnectionProvider : IConnectionProvider
{
    private readonly ConnectionFactory _factory;
    private readonly IConnection _connection;
    private bool _disposed;

    public ConnectionProvider(string url)
    {
        _factory = new ConnectionFactory
        {
            Uri = new Uri(url)
        };
        _connection = _factory.CreateConnection();
    }

    public IConnection GetConnection()
    {
        return _connection;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Protected implementation of Dispose pattern.
    protected virtual void Dispose(bool disposing)
    {
        if(_disposed) return;

        if(disposing) _connection?.Dispose();

        _disposed = true;
    }
}
