using Dapper;
using Ecom.Core.Domain;
using Microsoft.Data.SqlClient;

namespace Ecom.Orders.Api.Endpoints.ShoppingCart;

/// <summary>
/// Using Dapper ORM just cuz!
/// </summary>
public class FunDapperRepository(string connectionString)
{
    public async Task<ShoppingCartItem> AddOrUpdateCartItem(int customerId, int productId, int quantity)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.ExecuteAsync("""
            MERGE INTO ShoppingCartItems AS target
            USING (VALUES (@CustomerId, @ProductId, @ProductAttributesXml, @Quantity, GetUtcDate(), GetUtcDate(), Dateadd(minute, 30, GETUTCDATE()), 0)) 
                AS source (CustomerId, ProductId, ProductAttributesXml, Quantity, CreatedDateUtc, ModifiedDateUtc, ReserveInCartEndDateUtc, Deleted)
            ON target.CustomerId = source.CustomerId 
                AND target.ProductId = source.ProductId
                AND target.ReserveInCartEndDateUtc > GETUTCDATE()
                AND target.Deleted = 0
            WHEN MATCHED THEN
                UPDATE SET Quantity += source.Quantity
            WHEN NOT MATCHED THEN
                INSERT (CustomerId, ProductId, ProductAttributesXml, Quantity, CreatedDateUtc, ModifiedDateUtc, ReserveInCartEndDateUtc, Deleted)
                VALUES (source.CustomerId, source.ProductId, source.ProductAttributesXml, source.Quantity, source.CreatedDateUtc, source.ModifiedDateUtc, source.ReserveInCartEndDateUtc, source.Deleted);
            """,
            new
            {
                CustomerId = customerId,
                ProductId = productId,
                ProductAttributesXml = string.Empty,
                Quantity = quantity
            });

        return (await GetCartItems(customerId, productId)).First();
    }

    public async Task<IEnumerable<ShoppingCartItem>> GetCartItems(int customerId, int? productId = null)
    {
        using var connection = new SqlConnection(connectionString);
        return await connection.QueryAsync<ShoppingCartItem>("""
            SELECT 
                Id,
                CustomerId,
                ProductId,
                ProductAttributesXml,
                Quantity,
                ReserveInCartEndDateUtc,
                Deleted
            FROM dbo.ShoppingCartItems
            WHERE CustomerId = @CustomerId 
                AND Deleted = 0
                AND ReserveInCartEndDateUtc > GETUTCDATE()
                AND (@ProductId IS NULL OR ProductId = @ProductId)
            """, 
            new 
            { 
                CustomerId = customerId,
                ProductId = productId
            });
    }

    public async Task<ShoppingCartItem?> GetCartItemById(int shoppingCartItemId)
    {
        using var connection = new SqlConnection(connectionString);
        return await connection.QueryFirstOrDefaultAsync<ShoppingCartItem>("""
            SELECT
                Id,
                CustomerId,
                ProductId,
                ProductAttributesXml,
                Quantity,
                ReserveInCartEndDateUtc,
                Deleted
            FROM dbo.ShoppingCartItems
            WHERE Id = @ShoppingCartItemId
            """,
            new
            {
                ShoppingCartItemId = shoppingCartItemId
            });
    }

    public async Task RemoveCartItem(int shoppingCartItemId)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.ExecuteAsync("Update dbo.ShoppingCartItems set Deleted = 1 where Id = @Id", new { Id = shoppingCartItemId });
    }
}
