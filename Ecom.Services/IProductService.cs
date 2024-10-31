using Ecom.Core;
using Ecom.Core.Domain;

namespace Ecom.Services;

public interface IProductService
{
    Task<IPagedList<Product>> GetProductsAsync(string? category, int page, int pageSize);
    Task<Product> GetProductAsync(int id);
    Task<List<string>> GetAllCategoriesAsync();
}
