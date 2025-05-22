using System.ComponentModel.DataAnnotations;

namespace CatalogApi.DTOs;

public class CategoryRequest
{
    [Required()]
    [StringLength(100)]
    public string? Name { get; set; }
    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }
}