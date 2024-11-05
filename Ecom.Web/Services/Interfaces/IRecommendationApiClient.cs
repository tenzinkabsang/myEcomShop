using Ecom.Core.Domain;

namespace Ecom.Web.Services.Interfaces;

public interface IRecommendationApiClient
{
    /// <summary>
    /// This can be its own recommendation service. It is implemented in CatalogApi for simplicity!
    /// </summary>
    Task<IList<Product>> GetRecommendations(int currentProductId);
}
