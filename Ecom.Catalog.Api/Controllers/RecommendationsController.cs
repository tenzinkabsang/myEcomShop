using Ecom.Catalog.Api.Services;
using Ecom.Core.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Catalog.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecommendationsController(IRecommendationService recommendationService) : ControllerBase
{

    [HttpGet("{productId}")]
    public async Task<ActionResult<Product>> GetProduct(int productId)
    {
        var items = await recommendationService.GetRecommendations(productId);

        if (items is null || items.Count == 0)
            return NotFound();

        return Ok(items);
    }
}