using AutoMapper;
using Ecom.Core;
using Ecom.Web.Models;
using Ecom.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ecom.Web.Pages;

public class CartModel(ICatalogApiClient catalogApiClient, 
    ICartApiClient cartService,
    TestUser customer,
    IMapper mapper, 
    ILogger<CartModel> logger) : PageModel
{
    private const string BASE_URL = "/";

    public Cart Cart { get; set; } = new();

    public string ReturnUrl { get; set; } = BASE_URL;

    public async Task OnGet(string returnUrl)
    {
        ReturnUrl = returnUrl ?? BASE_URL;

        var cartItems = await cartService.GetShoppingCartItems(customer.CustomerId);

        logger.LogInformation("{ShoppingCart} retrieved", cartItems.ToJson());

        Cart = new Cart();
        foreach (var c in cartItems)
        {
            var p = await catalogApiClient.GetProductAsync(c.ProductId);
            Cart.CartItems.Add(new LineItem
            {
                CartLineId = c.Id,
                Quantity = c.Quantity,
                Product = mapper.Map<ProductViewModel>(p)
            });
        }
    }

    public async Task<IActionResult> OnPost(int productId, string returnUrl)
    {
        //var product = await productService.GetProductAsync(productId);
        //Cart.AddItem(mapper.Map<ProductViewModel>(product), 1);

        // Add cartItem to database.
        await cartService.AddOrUpdateCart(customer.CustomerId, productId, quantity: 1);

        return RedirectToPage(new { returnUrl });
    }

    public async Task<IActionResult> OnPostRemove(int productId, string returnUrl)
    {
        var cartItem = (await cartService.GetShoppingCartItems(customer.CustomerId))
            .FirstOrDefault(c => c.ProductId == productId);

        if (cartItem != null)
        {
            //var product = await productService.GetProductAsync(cartItem.ProductId);
            //Cart.RemoveLine(mapper.Map<ProductViewModel>(product));
            await cartService.RemoveItem(cartItem.Id);

            logger.LogInformation("{ShoppingCartItemId} removed for {Customer}", cartItem.Id, customer.ToJson());
        }

        return RedirectToPage(new { returnUrl });
    }
}
