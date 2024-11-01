namespace Ecom.Web.Models;

public class Cart
{
    public List<LineItem> CartItems { get; set; } = [];

    public virtual void AddItem(ProductViewModel product, int quantity)
    {
        LineItem? line = CartItems.FirstOrDefault(p => p.Product.Id == product.Id);

        if (line == null)
            CartItems.Add(new LineItem { Product = product, Quantity = quantity });
        else
            line.Quantity += quantity;
    }

    public bool IsEmpty => !CartItems.Any();

    public virtual void RemoveLine(ProductViewModel product) => CartItems.RemoveAll(c => c.Product.Id == product.Id);

    public virtual void Clear() => CartItems.Clear();

    public decimal CartTotal => CartItems.Sum(c => c.Product.Price * c.Quantity);
}

public class LineItem
{
    public int CartLineId { get; set; }
    public ProductViewModel Product { get; set; } = new();
    public int Quantity { get; set; }

    public string MainImageUrl => Product.Images.FirstOrDefault(img => img.IsMainImage)?.ImageUrl ?? string.Empty;

    public decimal Total => Quantity * Product.Price;
}
