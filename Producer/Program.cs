using RabbitMQ.Client;
using System.Text;

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

// Convert message to byte array
byte[] messageBodyBytes = Encoding.UTF8.GetBytes("Hello World");

// Publish a message to the exchange with the specified routing key
channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);

// Close the channel
channel.Close();

// Close the connection
connection.Close();