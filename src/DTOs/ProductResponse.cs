using System.ComponentModel.DataAnnotations;

namespace CatalogApi.DTOs;

public class ProductResponse
{
    [Required] 
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
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
    public DateTime RegistrationDate { get; set; } = DateTime.Now;
    public int CategoryId { get; set; }
}