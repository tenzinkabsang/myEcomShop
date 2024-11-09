using AutoMapper;
using Ecom.Core.Domain;
using Ecom.Web.Models;
using Ecom.Web.Services.Interfaces;

namespace Ecom.Web.Services;

public class CatalogApiClient(HttpClient httpClient, IMapper mapper) : ICatalogApiClient, IRecommendationApiClient
{
    public async Task<ProductListViewModel> GetProductsAsync(int page, int pageSize, string? category)
    {
        var response = await httpClient
            .GetFromJsonAsync<GetProductsResponse>($"products?page={page}&pageSize={pageSize}&category={category}");

        if (response is null)
            throw new Exception("Unable to find products for the given category");

        return new ProductListViewModel
        {
            Products = response.Products.Select(mapper.Map<ProductViewModel>).ToList(),
            PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = response.TotalCount
            },
            CurrentCategory = category
        };
    }

    public async Task<ProductViewModel> GetProductAsync(int id)
    {
        var product = await httpClient.GetFromJsonAsync<Product>($"products/{id}");

        if (product is null)
            throw new ArgumentNullException($"No product found for id: {id}");

        return mapper.Map<ProductViewModel>(product);
    }

    public async Task<IList<string>> GetAllCategoriesAsync()
    {
        var categories = await httpClient.GetFromJsonAsync<IList<string>>("categories");
        return categories ?? [];
    }

    public async Task<IList<ProductViewModel>> GetRecommendations(int currentProductId)
    {
        var recommendations = await httpClient.GetFromJsonAsync<IList<Product>>($"recommendations/{currentProductId}");
        return (recommendations ?? [])
            .Select(mapper.Map<ProductViewModel>).ToList();
    }

    #region Response Models

    private record GetProductsResponse(int TotalCount, int TotalPages, IList<Product> Products);

    #endregion
}
