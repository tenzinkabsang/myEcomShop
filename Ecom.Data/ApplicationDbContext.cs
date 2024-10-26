using Ecom.Core;
using Ecom.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Image> Images => Set<Image>();
    public DbSet<Customer> Customers => Set<Customer>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var item in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
        {
            if (item is IAuditableEntity auditableEntity)
            {
                auditableEntity.ModifiedDateUtc = DateTime.UtcNow;

                if (item.State == EntityState.Added)
                    auditableEntity.CreatedDateUtc = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
