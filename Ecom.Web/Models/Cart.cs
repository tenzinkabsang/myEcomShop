using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecom.Web.Models;

public record Cart
{
    public IList<LineItem> CartItems { get; set; } = [];
    public bool IsEmpty => CartItems.Count == 0;
    public decimal CartTotal => CartItems.Sum(c => c.Product.Price * c.Quantity);
}

public record LineItem
{
    public int CartLineId { get; set; }
    public ProductViewModel Product { get; set; } = new();
    public int Quantity { get; set; }

    [BindNever]
    public string? ProductAttributesXml { get; set; }
    public string MainImageUrl => Product.Images.FirstOrDefault(img => img.IsMainImage)?.ImageUrl ?? string.Empty;
    public decimal Total => Quantity * Product.Price;
}
