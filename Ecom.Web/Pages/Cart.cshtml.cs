using Ecom.Core.Extensions;
using Ecom.Web.Models;
using Ecom.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ecom.Web.Pages;

public class CartModel(ICartApiClient cartService,
    TestUser customer,
    ILogger<CartModel> logger) : PageModel
{
    private const string BASE_URL = "/";

    public Cart Cart { get; set; } = new();

    public string ReturnUrl { get; set; } = BASE_URL;

    public async Task OnGet(string returnUrl)
    {
        ReturnUrl = returnUrl ?? BASE_URL;
        Cart.CartItems = await cartService.GetShoppingCartItems(customer.CustomerId);
        logger.LogInformation("{ShoppingCart} retrieved", Cart.ToJson());
    }

    public async Task<IActionResult> OnPost(int productId, string returnUrl)
    {
        await cartService.AddOrUpdateCart(customer.CustomerId, productId, quantity: 1);
        return RedirectToPage(new { returnUrl });
    }

    public async Task<IActionResult> OnPostRemove(int shoppingCartItemId, string returnUrl)
    {
        await cartService.RemoveItem(shoppingCartItemId);
        logger.LogInformation("{ShoppingCartItemId} removed for {Customer}", shoppingCartItemId, customer.ToJson());
        return RedirectToPage(new { returnUrl });
    }
}
