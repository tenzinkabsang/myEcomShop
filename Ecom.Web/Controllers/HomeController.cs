using AutoMapper;
using Ecom.Web.Models;
using Ecom.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ecom.Web.Controllers;

public class HomeController(ICatalogApiClient catalogApiClient,
    IRecommendationApiClient recommendationApiClient,
    IConfiguration config,
    ILogger<HomeController> logger) : Controller
{
    private readonly int _pageSize = config.GetValue<int>("Home:PageSize");

    public async Task<IActionResult> Index(string? category, int page = 1)
    {
        var viewModel = await catalogApiClient.GetProductsAsync(page, _pageSize, category);
        return View(viewModel);
    }

    public async Task<IActionResult> Detail(int productId, string returnUrl)
    {
        logger.LogInformation("Getting product detail for {ProductId}", productId);

        var productTask = catalogApiClient.GetProductAsync(productId);
        var recommendationsTask = recommendationApiClient.GetRecommendations(productId);

        await Task.WhenAll(productTask, recommendationsTask);

        var viewModel = new ProductDetailViewModel
        {
            Product = productTask.Result,
            ReturnUrl = returnUrl,
            RecommendedItems = recommendationsTask.Result
        };

        return View(viewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
