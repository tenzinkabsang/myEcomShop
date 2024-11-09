using Ecom.Web.Models;

namespace Ecom.Web.Services.Interfaces;

public interface IRecommendationApiClient
{
    /// <summary>
    /// This can be its own recommendation service. It is implemented in CatalogApi for simplicity!
    /// </summary>
    Task<IList<ProductViewModel>> GetRecommendations(int currentProductId);
}
