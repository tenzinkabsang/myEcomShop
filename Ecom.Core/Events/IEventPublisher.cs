namespace Ecom.Core.Events;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event);
}
