using Ecom.Web.Services.Dtos;

namespace Ecom.Web.Services.Interfaces;

public interface ICartApiClient
{
    Task<IList<ShoppingCartItemDto>> GetShoppingCartItems(int customerId);

    Task<int?> AddOrUpdateCart(int customerId, int productId, int quantity);

    Task RemoveItem(int shoppingCartItemId);
}
