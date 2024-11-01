using Ecom.Core.Domain;

namespace Ecom.Services.Interfaces;

public interface ICartService
{
    Task<IList<ShoppingCartItem>> GetShoppingCartItems(int customerId);

    Task<int> AddItem(int customerId, int productId, int quantity);

    Task RemoveItem(int shoppingCartItemId);
}
