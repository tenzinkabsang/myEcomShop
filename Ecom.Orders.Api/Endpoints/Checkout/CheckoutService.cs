using Ecom.Core.Domain;
using Ecom.Data;

namespace Ecom.Orders.Api.Endpoints.Checkout;

public sealed class CheckoutService(IRepository<Order> orderRepository)
{
    public async Task<CreateOrderResponse> ProcessOrder(Order order)
    {
        await orderRepository.InsertAsync(order);
        return new CreateOrderResponse(order.Id, order.CustomerId);
    }
}



