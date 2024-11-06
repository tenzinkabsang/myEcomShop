using AutoMapper;

namespace Ecom.Orders.Api.Endpoints.ShoppingCart;

public sealed class ShoppingCartService(FunDapperRepository repository, IMapper mapper)
{
    public async Task<ShoppingCartItemDto> AddOrUpdateCartItem(int customerId, int productId, int quantity)
    {
        var item = await repository.AddOrUpdateCartItem(customerId, productId, quantity);
        return mapper.Map<ShoppingCartItemDto>(item);
    }

    public async Task<IList<ShoppingCartItemDto>> GetCartItems(int customerId)
    {
        var items = await repository.GetShoppingCart(customerId);
        return mapper.Map<IList<ShoppingCartItemDto>>(items);
    }

    public async Task<ShoppingCartItemDto?> GetCartItem(int shoppingCartItemId)
    {
        var item = await repository.GetCartItemById(shoppingCartItemId);
        return mapper.Map<ShoppingCartItemDto>(item);
    }

    public Task RemoveCartItem(int shoppingCartItemId) => repository.RemoveCartItem(shoppingCartItemId);
}
