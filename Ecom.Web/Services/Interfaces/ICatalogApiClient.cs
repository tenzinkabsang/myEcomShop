using Ecom.Core.Domain;

namespace Ecom.Web.Services.Interfaces;

public interface ICatalogApiClient
{
    Task<(int TotalCount, IList<Product> Products)> GetProductsAsync(int page, int pageSize, string? category);
    Task<Product> GetProductAsync(int id);
    Task<IList<string>> GetAllCategoriesAsync();
}
