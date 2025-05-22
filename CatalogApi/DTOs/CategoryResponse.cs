using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using CatalogApi.Models;

namespace CatalogApi.DTOs;

public class CategoryResponse
{
    public CategoryResponse()
    {
        Products = new Collection<ProductResponse>();
    }

    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<ProductResponse> Products { get; set; } 
}