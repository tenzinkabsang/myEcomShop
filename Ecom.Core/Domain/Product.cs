using System.ComponentModel.DataAnnotations.Schema;

namespace Ecom.Core.Domain;

public class Product : BaseEntity, ISoftDeletedEntity, IAuditableEntity
{
    public string Name { get; set; } = string.Empty;

    public string ShortDescription { get; set; } = string.Empty;

    public string FullDescription { get; set; } = string.Empty;

    public string Sku { get; set; } = string.Empty;

    [Column(TypeName = "decimal(8, 2)")]
    public decimal Price { get; set; }

    public string Category { get; set; } = string.Empty;

    public IList<Image> Images { get; set; } = [];

    public bool Deleted { get; set; }

    public DateTime CreatedDateUtc { get; set; }

    public DateTime ModifiedDateUtc { get; set; }
}
