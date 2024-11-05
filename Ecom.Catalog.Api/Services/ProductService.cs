using Ecom.Catalog.Api.Services;
using Ecom.Core;
using Ecom.Core.Domain;
using Ecom.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Services;

public class ProductService(IRepository<Product> productRepository, ILogger<ProductService> logger) : IProductService
{
    public async Task<IPagedList<Product>> GetProductsAsync(int page, int pageSize, string? category)
    {
        logger.LogInformation("Retrieving products for {Category}", category);

        var pagedProducts = await productRepository.GetAllPagedAsync(
            query => query
                // Load images with products
                .Include(x => x.Images)

                // Filter with category if provided
                .Where(p => category == null || p.Category == category)
                
                // Todo: Implement ordering based on user history (items favorited, viewed, etc.)
                .OrderBy(p => Guid.NewGuid()),
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
