namespace Ecom.Core.Domain;

public class ShoppingCartItem : BaseEntity, IAuditableEntity, ISoftDeletedEntity
{
    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    public string ProductAttributesXml { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public DateTime CreatedDateUtc { get; set; }

    public DateTime ModifiedDateUtc { get; set; }

    public DateTime ReserveInCartEndDateUtc { get; set; }

    public bool Deleted { get; set; }
}
