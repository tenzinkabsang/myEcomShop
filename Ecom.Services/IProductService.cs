using Ecom.Core;
using Ecom.Core.Domain;

namespace Ecom.Services;

public interface IProductService
{
    Task<IPagedList<Product>> GetProducts(string? category, int page, int pageSize);

    Task<Product> GetProduct(int id);
    Task<List<string>> GetAllCategoriesAsync();
}
