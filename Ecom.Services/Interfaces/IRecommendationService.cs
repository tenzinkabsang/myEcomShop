using Ecom.Core.Domain;

namespace Ecom.Services.Interfaces;

public interface IRecommendationService
{
    Task<IList<Product>> GetItemsFor(Product product);
}
