using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatalogApi.Models;

[Table("Categories")]
public class Category
{
    public Category()
    {
        Products = new Collection<Product>();
    }

    [Key]
    public int Id { get; set; }
    [Required()]
    [StringLength(100)]
    public string? Name { get; set; }
    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<Product> Products { get; set; }
}