using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CatalogApi.DTOs;

public class CategoryResponse
{
    public int Id { get; set; }
    [Required()]
    [StringLength(100)]
    public string? Name { get; set; }
    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IEnumerable<ProductResponse>? Products { get; set; }
    public bool Certo { get; set; } = true;
}