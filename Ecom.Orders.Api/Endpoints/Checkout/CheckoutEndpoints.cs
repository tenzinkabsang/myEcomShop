using Ecom.Core.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Orders.Api.Endpoints.Checkout;

public static class CheckoutEndpoints
{
    public static void MapCheckoutEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("checkout", async ([FromBody] Order data, CheckoutService checkoutService) =>
        {
            var newOrderInfo = await checkoutService.ProcessOrder(data);
            return Results.Created($"orders/{newOrderInfo.OrderId}", newOrderInfo);
        });
    }
}



