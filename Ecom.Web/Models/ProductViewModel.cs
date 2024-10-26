using Ecom.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace Ecom.Web.Models;

public class ProductViewModel
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Description { get; set; }

    public decimal Price { get; set; }

    public required string Category { get; set; }

    public IList<Image> Images { get; set; } = [];
}
