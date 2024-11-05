using Ecom.Core.Domain;
using Ecom.Web.Services.Dtos;
using Ecom.Web.Services.Interfaces;

namespace Ecom.Web.Services;

public class CatalogApiClient : ICatalogApiClient, IRecommendationApiClient
{
    private readonly HttpClient _httpClient;

    public CatalogApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(int TotalCount, IList<Product> Products)> GetProductsAsync(int page, int pageSize, string? category)
    {
        var response = await _httpClient
            .GetFromJsonAsync<GetProductsResponse>($"products?page={page}&pageSize={pageSize}&category={category}");

        if (response is null)
            throw new Exception("Unable to find products for the given category");

        return (response.TotalCount, response.Products);
    }

    public async Task<Product> GetProductAsync(int id)
    {
        var product = await _httpClient.GetFromJsonAsync<Product>($"products/{id}");

        if (product is null)
            throw new ArgumentNullException($"No product found for id: {id}");

        return product;
    }

    public async Task<IList<string>> GetAllCategoriesAsync()
    {
        var categories = await _httpClient.GetFromJsonAsync<IList<string>>("categories");

        return categories ?? [];
    }

    public async Task<IList<Product>> GetRecommendations(int currentProductId)
    {
        var recommendations = await _httpClient.GetFromJsonAsync<IList<Product>>($"recommendations/{currentProductId}");

        return recommendations ?? [];
    }
}
