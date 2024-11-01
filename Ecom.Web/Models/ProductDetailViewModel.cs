namespace Ecom.Web.Models;

public class ProductDetailViewModel
{
    public ProductViewModel Product { get; set; } = new();
    public string ReturnUrl { get; set; } = string.Empty;

    public ICollection<ProductViewModel> RecommendedItems { get; set; } = [];
}
