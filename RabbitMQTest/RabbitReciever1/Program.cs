using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbit Reciever1 App";

IConnection cnn = factory.CreateConnection();
IModel channel = cnn.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName = "DemoQueue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey);
channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (sender, args) =>
{
    Task.Delay(TimeSpan.FromSeconds(5)).Wait();
    var body = args.Body.ToArray();
    string msg = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Message Recieved : {msg}");

    channel.BasicAck(args.DeliveryTag, false);
};

var consumerTag = channel.BasicConsume(queueName, false, consumer);

Console.ReadLine();

channel.BasicCancel(consumerTag);

channel.Close();
cnn.Close();