namespace Ecom.Core.Domain;

public class OrderItem : BaseEntity, IAuditableEntity
{
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    public string? ProductAttributesXml { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime ModifiedDateUtc { get; set; }
}
