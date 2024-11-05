using Ecom.Core.Domain;
using Ecom.Core;

namespace Ecom.Web.Services.Interfaces;

public interface ICatalogApiClient
{
    Task<IPagedList<Product>> GetProductsAsync(int page, int pageSize, string? category);
    Task<Product> GetProductAsync(int id);
    Task<IList<string>> GetAllCategoriesAsync();
}
