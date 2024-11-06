namespace Ecom.Web.Configuration;

public record ApiSettings
{
    public required string CatalogApiBaseAddress { get; set; }
    public required string OrdersApiBaseAddress { get; set; }
    public required string ShoppingCartApiBaseAddress { get; set; }
}
