namespace Ecom.Web.Models;

public class ProductListViewModel
{
    public IList<ProductViewModel> Products { get; set; } = [];

    public PagingInfo PagingInfo { get; set; } = new();

    public string? CurrentCategory { get; set; }
}
