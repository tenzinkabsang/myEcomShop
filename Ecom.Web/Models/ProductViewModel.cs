using Ecom.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace Ecom.Web.Models;

public record ProductViewModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string ShortDescription { get; set; } = string.Empty;

    public string? FullDescription { get; set; }

    [Required]
    public string Sku { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    [Required]
    public string Category { get; set; } = string.Empty;

    public IList<Image> Images { get; set; } = [];
}
