using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// Create a new ConnectionFactory instance
ConnectionFactory factory = new();

// Set the URI of the RabbitMQ server
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

// Set a client-provided name for identification
factory.ClientProvidedName = "Rabbit Consumer";

// Create a new connection to the RabbitMQ server
using IConnection connection = factory.CreateConnection();

// Create a new channel over the connection
IModel channel = connection.CreateModel();

// Define exchange, routing key, and queue names
string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName = "DemoQueue";

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

// Set basic quality of service (QoS) settings to prefetch one message at a time
channel.BasicQos(0, 1, false);

// Create a consumer using EventingBasicConsumer
var consumer = new EventingBasicConsumer(channel);

// Register an event handler for receiving messages
consumer.Received += (sender, args) =>
{
    // Extract message body and convert to string
    var body = args.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);

    // Display received message
    Console.WriteLine($"Message Received: {message}");

    // Acknowledge the message delivery
    channel.BasicAck(args.DeliveryTag, false);
};

// Start consuming messages from the queue
// Parameters:
// queue: The name of the queue to consume from
// autoAck: false (Manual acknowledgment is enabled)
// consumer: The consumer to handle incoming messages
string consumerTag = channel.BasicConsume(queueName, false, consumer);

// Wait for user input before exiting
Console.ReadLine();

// Cancel consuming messages
channel.BasicCancel(consumerTag);

// Close the channel
channel.Close();

// Close the connection
connection.Close();