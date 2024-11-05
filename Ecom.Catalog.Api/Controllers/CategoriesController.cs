using Ecom.Catalog.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Catalog.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IList<string>>> GetCategories()
    {
        var categories = await productService.GetAllCategoriesAsync();

        if (categories == null || !categories.Any())
            return NotFound();

        return Ok(categories);
    }
}