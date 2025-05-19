using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using CatalogApi.Validations;

namespace CatalogApi.Models;

[Table("Products")]
public class Product
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    [FirstCapitalLetter]
    public string? Name { get; set; }
    [Required]
    [StringLength(300)]
    public string? Description { get; set; }
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }
    public float Stock { get; set; }
    public DateTime RegistrationDate { get; set; }
    public int CategoryId { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Category? Category { get; set; }
}