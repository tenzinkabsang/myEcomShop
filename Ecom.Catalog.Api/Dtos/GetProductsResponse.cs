using Ecom.Core.Domain;

namespace Ecom.Catalog.Api.Dtos;

public record GetProductsResponse
{
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
    public IList<Product> Products { get; init; } = [];
}
