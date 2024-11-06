using System.Text.Json;
using Ecom.Web.Services.Dtos;
using Ecom.Web.Services.Interfaces;

namespace Ecom.Web.Services;

public class CartApiClient(HttpClient httpClient) : ICartApiClient
{
    public async Task<int?> AddOrUpdateCart(int customerId, int productId, int quantity)
    {
        var httpResponse = await httpClient.PostAsJsonAsync("items", new { customerId, productId, quantity });
        using var stream = await httpResponse.Content.ReadAsStreamAsync();
        var item = await JsonSerializer.DeserializeAsync<ShoppingCartItemDto>(stream);
        return item?.Id;
    }

    public async Task<IList<ShoppingCartItemDto>> GetShoppingCartItems(int customerId)
    {
        return await httpClient.GetFromJsonAsync<List<ShoppingCartItemDto>>($"items?customerId={customerId}") ?? [];
    }

    public Task RemoveItem(int shoppingCartItemId) => httpClient.DeleteAsync($"items/{shoppingCartItemId}");
}
