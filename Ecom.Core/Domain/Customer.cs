
namespace Ecom.Core.Domain;

public class Customer : BaseEntity, ISoftDeletedEntity, IAuditableEntity
{
    public Guid? CustomerGuid { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public Address BillingAddress { get; set; } = new();

    public bool Deleted { get; set; }

    public DateTime CreatedDateUtc { get; set; }

    public DateTime ModifiedDateUtc { get; set; }
}

