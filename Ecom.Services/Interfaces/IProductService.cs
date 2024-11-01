using Ecom.Core;
using Ecom.Core.Domain;

namespace Ecom.Services.Interfaces;

public interface IProductService
{
    Task<IPagedList<Product>> GetProductsAsync(string? category, int page, int pageSize);
    Task<Product> GetProductAsync(int id);
    Task<IList<string>> GetAllCategoriesAsync();
}