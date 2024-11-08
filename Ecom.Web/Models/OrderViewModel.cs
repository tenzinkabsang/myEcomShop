using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Ecom.Web.Models;

public class OrderViewModel
{
    [BindNever]
    public int OrderId { get; set; }

    [BindNever]
    public IList<LineItem> Items { get; set; } = [];

    [Required(ErrorMessage = "Please enter your first name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Please enter your last name")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Please enter your email address")]
    public string Email { get; set; }

    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Please enter the first address")]
    public string Line1 { get; set; }

    [Required(ErrorMessage = "Please enter a city name")]
    public string City { get; set; }

    public string State { get; set; }

    public string Zip { get; set; }

    [DisplayName("Gift wrap these items")]
    public bool GiftWrap { get; set; }

    [BindNever]
    public bool IsShipped { get; set; }

    [BindNever]
    public string FullName => $"{FirstName} {LastName}";

    public decimal OrderTotal() => Items.Sum(i => i.Product.Price * i.Quantity);
}
