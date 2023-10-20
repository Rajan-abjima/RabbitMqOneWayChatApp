using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.ComponentModel.Design;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.UserName = "guest";
factory.Password = "guest";
factory.Port = 5672;
factory.HostName = "localhost";
factory.VirtualHost = "/";

IConnection cnn = factory.CreateConnection();

IModel channel = cnn.CreateModel();

string exchangeName = "TrialExchange";
string routingkey = "test-routing-key";
string queueName = "TrialQueue";
channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, true, false, false, null);
channel.QueueBind(queueName, exchangeName, routingkey, null);
channel.BasicQos(0,1,false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, args) =>
{
    Task.Delay(TimeSpan.FromSeconds(1)).Wait();
    var body = args.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"MessageRecevied: {message}");
    channel.BasicAck(args.DeliveryTag, false);
};

string consumerTag = channel.BasicConsume(queueName, false, consumer);
Console.ReadLine();

channel.BasicCancel(consumerTag);

channel.Close();
cnn.Close();