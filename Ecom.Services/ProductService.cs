using Ecom.Core;
using Ecom.Core.Domain;
using Ecom.Data;
using Ecom.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ecom.Services;

public class ProductService(IRepository<Product> productRepository, ILogger<ProductService> logger) : IProductService
{
    public async Task<IPagedList<Product>> GetProductsAsync(string? category, int page, int pageSize)
    {
        logger.LogInformation("Retrieving products for {Category}", category);

        var pagedProducts = await productRepository.GetAllPagedAsync(
            query => query.Include(x => x.Images).Where(p => category == null || p.Category == category).OrderBy(p => p.Id),
            pageIndex: page,
            pageSize: pageSize
            );

        return pagedProducts;
    }

    public async Task<Product> GetProductAsync(int id)
    {
        logger.LogInformation("Retrieving product with {ProductId}", id);

        var product = await productRepository.GetByIdAsync(id, query => query.Include(q => q.Images));

        if (product is null)
            throw new ArgumentNullException($"No product found for id: {id}");

        return product;
    }

    public async Task<IList<string>> GetAllCategoriesAsync()
    {
        var categories = (await productRepository.GetAllAsync())
            .Select(p => p.Category)
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        logger.LogInformation("Categories retrieved: {Categories}", string.Join(",", categories));

        return categories;
    }
}
