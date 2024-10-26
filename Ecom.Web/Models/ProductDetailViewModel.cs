using Ecom.Core.Domain;

namespace Ecom.Web.Models;

public class ProductDetailViewModel
{
    public Product Product { get; set; } = new();
    public string ReturnUrl { get; set; } = string.Empty;

    public ICollection<Product> RecommendedItems { get; set; } = new List<Product>();
}
