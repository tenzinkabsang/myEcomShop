using Ecom.Core.Domain;
using Ecom.Data;
using Ecom.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Ecom.Services;

public class CartService : ICartService
{
    private readonly IRepository<ShoppingCartItem> _cartRepository;
    private readonly ILogger<CartService> _logger;

    public CartService(IRepository<ShoppingCartItem> cartRepository, ILogger<CartService> logger)
    {
        _cartRepository = cartRepository;
        _logger = logger;
    }

    public async Task<int> AddItem(int customerId, int productId, int quantity)
    {
        var existingCartItem = (await GetShoppingCartItems(customerId))
            .FirstOrDefault(c => c.ProductId == productId);

        if(existingCartItem != null)
        {
            existingCartItem.Quantity += quantity;
            await _cartRepository.UpdateAsync(existingCartItem);
            return existingCartItem.Id;
        }

        var cartItem = new ShoppingCartItem { CustomerId = customerId, ProductId = productId, Quantity = quantity, ReserveInCartEndDateUtc = DateTime.UtcNow.AddMinutes(30) };
        await _cartRepository.InsertAsync(cartItem);
        return cartItem.Id;
    }

    public Task RemoveItem(int shoppingCartItemId)
    {
        return _cartRepository.DeleteByIdAsync(shoppingCartItemId);
    }

    public async Task<IList<ShoppingCartItem>> GetShoppingCartItems(int customerId)
    {
        var shoppingCartItems = await _cartRepository.GetAllAsync(sc => 
                sc.Where(x => x.CustomerId == customerId && DateTime.UtcNow <= x.ReserveInCartEndDateUtc));

        return shoppingCartItems;
    }
}