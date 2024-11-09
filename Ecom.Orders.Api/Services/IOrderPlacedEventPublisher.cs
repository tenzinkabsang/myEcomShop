using Ecom.Core.Domain;

namespace Ecom.Orders.Api.Services;

public interface IOrderPlacedEventPublisher
{
    Task Publish(Order order);
}
