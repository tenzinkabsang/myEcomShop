using Ecom.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Web.Components;

public class NavigationMenuViewComponent(IProductService productService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await productService.GetAllCategoriesAsync();
        ViewBag.SelectedCategory = RouteData?.Values["category"];
        return View(categories);
    }
}
