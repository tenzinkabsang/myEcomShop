namespace Ecom.Core;

public interface IAuditableEntity
{
    DateTime CreatedDateUtc { get; set; }

    DateTime ModifiedDateUtc { get; set; }
}
