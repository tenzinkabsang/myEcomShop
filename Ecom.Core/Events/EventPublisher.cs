using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ecom.Core.Events;

public sealed class EventPublisher(IServiceProvider serviceProvider) : IEventPublisher
{
    public async Task PublishAsync<TEvent>(TEvent @event)
    {
        var consumers = serviceProvider.CreateAsyncScope().ServiceProvider.GetServices<IConsumer<TEvent>>().ToList();

        foreach (var consumer in consumers)
        {
            try
            {
                await consumer.HandleEventAsync(@event);
            }
            catch (Exception exception)
            {
                try
                {
                    //log error, we put in to nested try-catch to prevent possible cyclic (if some error occurs)
                    var logger = serviceProvider.CreateAsyncScope().ServiceProvider.GetService<ILogger>();
                    if (logger == null)
                        return;

                    logger.LogError(exception.Message, exception);
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
