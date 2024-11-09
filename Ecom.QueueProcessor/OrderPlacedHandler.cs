using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Ecom.QueueProcessor;

public class OrderPlacedHandler : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName = "order.received";
    private EventingBasicConsumer _consumer;
    private readonly ILogger<OrderPlacedHandler> _logger;

    public OrderPlacedHandler(IConfiguration config, ILogger<OrderPlacedHandler> logger)
    {
        var factory = new ConnectionFactory { HostName = config.GetValue<string>("RabbitMqHost") };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(_queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        _consumer = new EventingBasicConsumer(_channel);
        _consumer.Received += ProcessOrderPlacedEvent;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(10000, stoppingToken);
            _channel.BasicConsume(_queueName, autoAck: true, consumer: _consumer);
        }
    }

    /// <summary>
    /// Handle order processed event (emails, fulfillment, commissions, etc.)
    /// </summary>
    private void ProcessOrderPlacedEvent(object? sender, BasicDeliverEventArgs e)
    {
        var order = JsonSerializer.Deserialize<OrderPlacedInfo>(e.Body.ToArray());
        _logger.LogInformation("OrderQueueHandler processing {Order}", order);
    }

    private record OrderPlacedInfo(int OrderId, int CustomerId);
}
