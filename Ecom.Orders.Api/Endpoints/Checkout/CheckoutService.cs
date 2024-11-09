using Ecom.Core.Domain;
using Ecom.Data;
using Ecom.Orders.Api.Services;

namespace Ecom.Orders.Api.Endpoints.Checkout;

public sealed class CheckoutService(IRepository<Order> orderRepository, IOrderPlacedEventPublisher eventPublisher)
{
    public async Task<CreateOrderResponse> ProcessOrder(Order order)
    {
        await orderRepository.InsertAsync(order);
        await eventPublisher.Publish(order);
        return new CreateOrderResponse(order.Id, order.CustomerId);
    }
}



