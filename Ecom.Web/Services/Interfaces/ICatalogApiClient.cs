using Ecom.Core.Domain;
using Ecom.Web.Models;

namespace Ecom.Web.Services.Interfaces;

public interface ICatalogApiClient
{
    Task<ProductListViewModel> GetProductsAsync(int page, int pageSize, string? category);
    Task<ProductViewModel> GetProductAsync(int id);
    Task<IList<string>> GetAllCategoriesAsync();
}
