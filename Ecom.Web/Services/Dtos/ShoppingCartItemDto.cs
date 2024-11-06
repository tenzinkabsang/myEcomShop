namespace Ecom.Web.Services.Dtos;

public record ShoppingCartItemDto
{
    public int Id { get; init; }
    public int CustomerId { get; init; }
    public int ProductId { get; init; }
    public string ProductAttributesXml { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public DateTime ReserveInCartEndDateUtc { get; init; }
    public bool Deleted { get; init; }
}