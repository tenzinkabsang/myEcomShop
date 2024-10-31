using Ecom.Services.Interfaces;
using Ecom.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ecom.Web.Controllers;

public class HomeController(IProductService productService,
    IRecommendationService recommendationService,
    IConfiguration config,
    ILogger<HomeController> logger) : Controller
{
    private readonly int _pageSize = config.GetValue<int>("Home:PageSize");

    public async Task<IActionResult> Index(string? category, int page = 1)
    {
        var products = await productService.GetProductsAsync(category, page, _pageSize);

        var viewModel = new ProductListViewModel
        {
            Products = [.. products],
            PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = _pageSize,
                TotalItems = products.TotalCount
            },
            CurrentCategory = category
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Detail(int productId, string returnUrl)
    {
        var product = await productService.GetProductAsync(productId);

        var recommendations = await recommendationService.GetItemsFor(product);

        var viewModel = new ProductDetailViewModel
        {
            Product = product,
            ReturnUrl = returnUrl,
            RecommendedItems = recommendations
        };

        return View(viewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
