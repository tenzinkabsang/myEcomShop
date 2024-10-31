using Ecom.Core.Domain;
using Ecom.Data;
using Ecom.Services.Interfaces;

namespace Ecom.Services;

public class CartService : ICartService
{
    private readonly IRepository<ShoppingCartItem> _cartRepository;

    public CartService(IRepository<ShoppingCartItem> cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<int> AddItem(int customerId, int productId, int quantity)
    {
        var cartItem = new ShoppingCartItem
        {
            CustomerId = customerId,
            ProductId = productId,
            Quantity = quantity
        };

        await _cartRepository.InsertAsync(cartItem);
        return cartItem.Id;
    }

    public Task DeleteItem(int shoppingCartItemId)
    {
        return _cartRepository.DeleteByIdAsync(shoppingCartItemId);
    }

    public async Task<IList<ShoppingCartItem>> GetShoppingCartItems(int customerId)
    {
        var shoppingCartItems = await _cartRepository.GetAllAsync(sc => 
                sc.Where(x => x.Customer.Id == customerId && DateTime.UtcNow <= x.ReserveInCartEndDateUtc), 
                includeDeleted: false);

        return shoppingCartItems;
    }
}