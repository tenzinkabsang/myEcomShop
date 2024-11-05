using Ecom.Core.Domain;

namespace Ecom.Catalog.Api.Services;

public interface IRecommendationService
{
    Task<IList<Product>> GetRecommendations(int productId);
}