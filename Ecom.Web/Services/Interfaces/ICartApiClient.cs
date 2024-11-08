using Ecom.Web.Models;

namespace Ecom.Web.Services.Interfaces;

public interface ICartApiClient
{
    Task<IList<LineItem>> GetShoppingCartItems(int customerId);

    Task<int?> AddOrUpdateCart(int customerId, int productId, int quantity);

    Task RemoveItem(int shoppingCartItemId);
}
