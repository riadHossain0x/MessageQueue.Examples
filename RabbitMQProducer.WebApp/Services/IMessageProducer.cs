using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMQProducer.WebApp.Services;

// Note: this is a basic implementation
public interface IMessageProducer
{
    void SendMessage<T>(T message);
}

public class MessageProducer : IMessageProducer
{
    public void SendMessage<T>(T message)
    {
        // Create a new ConnectionFactory instance
        ConnectionFactory factory = new();

        // Set the URI of the RabbitMQ server
        factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

        // Set a client-provided name for identification
        factory.ClientProvidedName = "Rabbit Producer";

        // Create a new connection to the RabbitMQ server
        using IConnection connection = factory.CreateConnection();

        // Create a new channel over the connection
        IModel channel = connection.CreateModel();

        // Define exchange, routing key, and queue names
        string exchangeName = "WebAppExchange";
        string routingKey = "webapp-routing-key";
        string queueName = "WebAppQueue";

        // Declare the exchange with the given name and type
        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);

        // Declare the queue with the given name
        // Arguments:
        // durable: false (Queue will not survive a broker restart)
        // exclusive: false (Queue can be accessed by other connections)
        // autoDelete: false (Queue will not be deleted when the last consumer unsubscribes)
        // arguments: null (No additional arguments)
        channel.QueueDeclare(queueName, false, false, false, null);

        // Bind the queue to the exchange with the specified routing key
        channel.QueueBind(queueName, exchangeName, routingKey, null);

        // Convert object into json string
        string jsonString = JsonSerializer.Serialize(message);
        // Convert jsonString to byte array
        byte[] messageBodyBytes = Encoding.UTF8.GetBytes(jsonString);

        // Publish a message to the exchange with the specified routing key
        channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);

        // Close the channel
        channel.Close();

        // Close the connection
        connection.Close();
    }
}
