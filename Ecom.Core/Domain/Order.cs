namespace Ecom.Core.Domain;

public class Order : BaseEntity, ISoftDeletedEntity, IAuditableEntity
{
    public bool GiftWrap { get; set; }

    public Address ShippingAddress { get; set; } = new();

    public IList<OrderItem> OrderItems { get; set; } = [];

    public bool IsShipped { get; set; }

    public bool Deleted { get; set; }

    public int CustomerId { get; set; }

    public DateTime CreatedDateUtc { get; set; }

    public DateTime ModifiedDateUtc { get; set; }
}

