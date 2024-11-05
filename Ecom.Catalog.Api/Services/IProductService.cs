using Ecom.Core;
using Ecom.Core.Domain;

namespace Ecom.Catalog.Api.Services;

public interface IProductService
{
    Task<IPagedList<Product>> GetProductsAsync(int page, int pageSize, string? category);
    Task<Product> GetProductAsync(int id);
    Task<IList<string>> GetAllCategoriesAsync();
}