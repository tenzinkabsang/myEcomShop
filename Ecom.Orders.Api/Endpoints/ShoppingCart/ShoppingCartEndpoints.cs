using Microsoft.AspNetCore.Mvc;

namespace Ecom.Orders.Api.Endpoints.ShoppingCart;

public static class ShoppingCartEndpoints
{
    public static void MapShoppingCartEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("cart/items", async ([FromBody] AddOrUpdateRequest data , ShoppingCartService cartService) =>
        {
            var cartItem = await cartService.AddOrUpdateCartItem(data.CustomerId, data.ProductId, data.Quantity);
            return Results.Created($"cart/items/{cartItem.Id}", cartItem);
        });

        app.MapDelete("cart/items/{shoppingCartItemId}", async (int shoppingCartItemId, ShoppingCartService cartService) =>
        {
            await cartService.RemoveCartItem(shoppingCartItemId);
            return Results.NoContent();
        });

        app.MapGet("cart/items", async (int customerId, ShoppingCartService cartService) =>
        {
            var cartItems = await cartService.GetCartItems(customerId);
            return Results.Ok(cartItems);
        });

        app.MapGet("cart/items/{shoppingCartItemId}", async (int shoppingCartItemId, ShoppingCartService cartService) =>
        {
            var cartItem = await cartService.GetCartItem(shoppingCartItemId);
            return cartItem is null ? Results.NotFound() : Results.Ok(cartItem);
        });
    }

    private record AddOrUpdateRequest(int CustomerId, int ProductId, int Quantity);
}



