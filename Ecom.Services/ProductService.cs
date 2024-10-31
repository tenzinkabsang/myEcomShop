using Ecom.Core;
using Ecom.Core.Domain;
using Ecom.Data;
using Ecom.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Services;

public class ProductService(IRepository<Product> productRepository) : IProductService
{
    public async Task<IPagedList<Product>> GetProductsAsync(string? category, int page, int pageSize)
    {
        var pagedProducts = await productRepository.GetAllPagedAsync(
            query => query.Include(x => x.Images).Where(p => category == null || p.Category == category).OrderBy(p => p.Id),
            pageIndex: page,
            pageSize: pageSize,
            includeDeleted: false
            );

        return pagedProducts;
    }

    public async Task<Product> GetProductAsync(int id)
    {
        var product = await productRepository.GetByIdAsync(id, includeDeleted: false);

        if (product is null)
            throw new ArgumentNullException($"No product found for id: {id}");

        return product;
    }

    public async Task<IList<string>> GetAllCategoriesAsync()
    {
        return (await productRepository.GetAllAsync())
            .Select(p => p.Category)
            .Distinct()
            .OrderBy(x => x)
            .ToList();
    }
}
