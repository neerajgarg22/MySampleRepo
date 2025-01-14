using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Configuration;


namespace NotificationService
{
    public class RabbitMQListener : BackgroundService
    {
        private readonly IConfiguration configuration;

        public RabbitMQListener(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = new Uri(configuration.GetValue<string>("RabbitMQUrl"));//Pull it from secret file
            factory.ClientProvidedName = "Notification Service - Order placed";
            IConnection cnn = await factory.CreateConnectionAsync();

            IChannel channel = await cnn.CreateChannelAsync();


            string exchangeName = "DemoExchange";
            string routingKey = "demo-routing-key";
            string queueName = "DemoQueue";

            await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct);
            await channel.QueueDeclareAsync(queueName, false, false, false, null);

            await channel.QueueBindAsync(queueName, exchangeName, routingKey, null);
            await channel.BasicQosAsync(0, 1, false);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (sender, args) =>
            {
                var body = args.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                if(string.Equals(message, "OrderPlaced"))
                {
                    Console.WriteLine("Order placed successfully");
                }
                await channel.BasicAckAsync(args.DeliveryTag, false);
            };
            await channel.BasicConsumeAsync(queueName,false,consumer);
            await Task.Delay(Timeout.Infinite, stoppingToken);
            //await channel.BasicCancelAsync(consumerTag);
            //await channel.CloseAsync();
            //await cnn.CloseAsync();
        }

    }
}
