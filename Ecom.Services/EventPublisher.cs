using Ecom.Core.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Ecom.Services;

public sealed class EventPublisher : IEventPublisher
{
    private readonly IServiceProvider _serviceProvider;

    public EventPublisher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task PublishAsync<TEvent>(TEvent @event)
    {
        //var consumers = _serviceProvider.CreateAsyncScope().ServiceProvider.GetServices<IConsumer<TEvent>>().ToList();

        await Task.Delay(2);
    }
}
