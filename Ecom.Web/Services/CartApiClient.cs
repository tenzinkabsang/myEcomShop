using System.Text.Json;
using AutoMapper;
using Ecom.Core.Extensions;
using Ecom.Web.Models;
using Ecom.Web.Services.Interfaces;

namespace Ecom.Web.Services;

public class CartApiClient(HttpClient httpClient, ICatalogApiClient catalogApiClient, IMapper mapper) : ICartApiClient
{
    public async Task<int?> AddOrUpdateCart(int customerId, int productId, int quantity)
    {
        var httpResponse = await httpClient.PostAsJsonAsync("items", new { customerId, productId, quantity });
        using var stream = await httpResponse.Content.ReadAsStreamAsync();
        var item = await JsonSerializer.DeserializeAsync<ShoppingCartItemDto>(stream);
        return item?.Id;
    }

    public async Task<IList<LineItem>> GetShoppingCartItems(int customerId)
    {
        return await (await httpClient.GetFromJsonAsync<List<ShoppingCartItemDto>>($"items?customerId={customerId}") ?? [])
            .SelectAwait(async item =>
            {
                var p = await catalogApiClient.GetProductAsync(item.ProductId);
                return new LineItem
                {
                    CartLineId = item.Id,
                    Quantity = item.Quantity,
                    Product = mapper.Map<ProductViewModel>(p)
                };
            }).ToListAsync();
    }

    public Task RemoveItem(int shoppingCartItemId) => httpClient.DeleteAsync($"items/{shoppingCartItemId}");

    #region Response Models
    
    private record ShoppingCartItemDto(
        int Id,
        int CustomerId,
        int ProductId,
        string ProductAttributesXml,
        int Quantity,
        DateTime ReserveInCartEndDateUtc,
        bool Deleted
        );

    #endregion
}
