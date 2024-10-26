using Ecom.Core.Domain;
using Ecom.Data;

namespace Ecom.Services;

public sealed class RecommendationService : IRecommendationService
{
    private const int ITEM_COUNT = 5;

    private readonly IRepository<Product> _productRepository;

    public RecommendationService(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IList<Product>> GetItemsFor(Product product)
    {
        return await _productRepository.GetAllAsync(
            query => query.Where(p => p.Category == product.Category).Take(ITEM_COUNT), includeDeleted: false
            );
    }
}
