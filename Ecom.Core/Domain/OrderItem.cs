namespace Ecom.Core.Domain;

public class OrderItem : BaseEntity, IAuditableEntity
{
    public int Quantity { get; set; }
    public Product Product { get; set; } = new();
    
    public DateTime CreatedDateUtc { get; set; }

    public DateTime ModifiedDateUtc { get; set; }
}
