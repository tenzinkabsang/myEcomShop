using System.Text.Json;
using Ecom.Core.Domain;
using RabbitMQ.Client;

namespace Ecom.Orders.Api.Services;

public class OrderPlacedEventPublisher : IOrderPlacedEventPublisher, IDisposable
{
    private IConnection _connection;
    private IModel _channel;
    private string _queueName = "order.received";

    public OrderPlacedEventPublisher(IConfiguration config)
    {
        var factory = new ConnectionFactory { HostName = config.GetValue<string>("RabbitMqHost") };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(_queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public async Task Publish(Order order)
    {
        var messageBytes = JsonSerializer.SerializeToUtf8Bytes(new { OrderId = order.Id, CustomerId = order.CustomerId });
        await Task.Run(() => 
        _channel.BasicPublish(
            exchange: string.Empty, 
            routingKey: _queueName, 
            mandatory: false, 
            basicProperties: null, 
            body: messageBytes
            ));
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
