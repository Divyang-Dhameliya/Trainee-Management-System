using System.Text;
using System.Text.Json;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

public class RabbitMqPublisher : IMessagePublisher
{
    private readonly RabbitMqOptions _options;

    public RabbitMqPublisher(IOptions<RabbitMqOptions> options)
    {
        _options = options.Value;
    }

    public async Task PublishAsync<T>(T message)
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.Host,
            Port = _options.Port,
            UserName = _options.Username,
            Password = _options.Password
        };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        string dlxExchange = "orders.dlx";

        var queueArguments = new Dictionary<string, object?>
        {
            { "x-queue-type", "quorum" },
            { "x-delivery-limit", 2 }, 
            { "x-dead-letter-exchange", dlxExchange },
            { "x-dead-letter-routing-key", _options.QueueName }
        };

        await channel.QueueDeclareAsync(
            queue: _options.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: queueArguments
        );

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: _options.QueueName,
            mandatory: false,
            basicProperties: new BasicProperties { Persistent = true },
            body: body
        );
    }
}