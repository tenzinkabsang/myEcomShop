using AutoMapper;
using Ecom.Core.Domain;
using Ecom.Web.Models;
using Ecom.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ecom.Web.Controllers;

public class OrderController : Controller
{
    private readonly IOrderApiClient _orderApiClient;
    private readonly ICartApiClient _cartService;
    private readonly TestUser _customer;
    private readonly IMapper _mapper;

    public OrderController(IOrderApiClient orderApiClient,
        ICartApiClient cartService,
        TestUser customer, 
        IMapper mapper)
    {
        _orderApiClient = orderApiClient;
        _cartService = cartService;
        _customer = customer;
        _mapper = mapper;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ViewBag.Cart = new Cart { CartItems = await _cartService.GetShoppingCartItems(_customer.CustomerId) };
        await base.OnActionExecutionAsync(context, next);
    }

    public IActionResult Checkout()
    {
        return View(new OrderViewModel());
    }

    [HttpPost]
    public IActionResult Checkout(OrderViewModel model)
    {
        if (!ViewBag.Cart.CartItems.Any())
            ModelState.AddModelError("", "Sorry, your cart is empty!");

        if (!ModelState.IsValid)
            return View();
        else
        {
            model.Items = ViewBag.Cart.CartItems;

            int orderId = _orderApiClient.ProcessCheckout(_mapper.Map<Order>(model));

            return RedirectToPage("/Completed", new { OrderId = orderId });
        }
    }
}
