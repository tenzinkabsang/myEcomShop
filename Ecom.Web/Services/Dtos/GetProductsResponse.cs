using Ecom.Core.Domain;

namespace Ecom.Web.Services.Dtos;

public record GetProductsResponse
{
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
    public IList<Product> Products { get; init; } = [];
}
