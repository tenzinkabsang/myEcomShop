using Ecom.Core;
using Ecom.Core.Domain;
using Ecom.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Catalog.Api.Services;

public sealed class RecommendationService(IRepository<Product> productRepository, ILogger<RecommendationService> logger) : IRecommendationService
{
    private const int ITEM_COUNT = 5;

    public async Task<IList<Product>> GetRecommendations(int productId)
    {
        var product = await productRepository.GetByIdAsync(productId);

        logger.LogInformation("Getting recommendentaions for {Product}", product.ToJson());
        return await productRepository.GetAllAsync(
            query => query
                .Include(p => p.Images)
                .Where(p => p.Id != product.Id && p.Category == product.Category)
                .Take(ITEM_COUNT)
            );
    }
}
