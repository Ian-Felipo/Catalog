using System.ComponentModel.DataAnnotations;
using CatalogApi.Validations;

namespace CatalogApi.DTOs;

public class ProductResponse
{
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    [FirstCapitalLetter]
    public string? Name { get; set; }
    [Required]
    [StringLength(300)]
    public string? Description { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }
    public float Stock { get; set; }
    [Required]
    public int CategoryId { get; set; }
}