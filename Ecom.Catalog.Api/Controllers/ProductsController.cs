using Ecom.Catalog.Api.Dtos;
using Ecom.Catalog.Api.Services;
using Ecom.Core;
using Ecom.Core.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Catalog.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetProductsResponse>> GetProducts(int page = 1, int pageSize = 20, string? category = null)
    {
        var products = await productService.GetProductsAsync(page, pageSize, category);

        if (products == null || !products.Any())
            return NotFound();

        return Ok(new GetProductsResponse
        {
            TotalCount = products.TotalCount,
            TotalPages = products.TotalPages,
            Products = products
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await productService.GetProductAsync(id);

        if (product is null)
            return NotFound();

        return Ok(product);
    }
}
