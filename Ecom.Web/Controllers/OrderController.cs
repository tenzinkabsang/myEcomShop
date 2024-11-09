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

    public async Task<IActionResult> Checkout()
    {
        ViewBag.Cart = new Cart { CartItems = await _cartService.GetShoppingCartItems(_customer.CustomerId) };
        return View(new OrderViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(OrderViewModel model)
    {
        var cartItems = await _cartService.GetShoppingCartItems(_customer.CustomerId);

        if (!cartItems.Any())
            ModelState.AddModelError("", "Sorry, your cart is empty!");

        if (!ModelState.IsValid)
            return View();

        model.Items = cartItems;
        model.CustomerId = _customer.CustomerId;
        int? orderId = await _orderApiClient.ProcessCheckout(model);
        return RedirectToPage("/Completed", new { OrderId = orderId });
    }
}
