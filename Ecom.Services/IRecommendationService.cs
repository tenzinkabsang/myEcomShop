using Ecom.Core.Domain;

namespace Ecom.Services;

public interface IRecommendationService
{
    Task<IList<Product>> GetItemsFor(Product product);
}
