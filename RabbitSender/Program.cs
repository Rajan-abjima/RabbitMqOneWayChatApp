using RabbitMQ.Client;
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
channel.QueueBind(queueName, exchangeName, routingkey,null);

//for (int i = 1; i <= 10; i++)
//{
//    Console.WriteLine($"Sending Test Message {i}");
//    byte[] testMessage = Encoding.UTF8.GetBytes($"TestOfRabbitMQ {i}");
//    channel.BasicPublish(exchangeName, routingkey, null, testMessage);
//    Thread.Sleep(1000);
//}

Console.WriteLine("Please write a message for receiver:");
var messageString = Console.ReadLine();
byte[] testMessage = Encoding.UTF8.GetBytes($"{messageString}");
channel.BasicPublish(exchangeName, routingkey, null, testMessage);

channel.Close();
cnn.Close();

