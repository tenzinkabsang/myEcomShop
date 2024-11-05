using Ecom.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Web.Components;

public class NavigationMenuViewComponent(ICatalogApiClient catalogApiClient) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await catalogApiClient.GetAllCategoriesAsync();
        ViewBag.SelectedCategory = RouteData?.Values["category"];
        return View(categories);
    }
}
