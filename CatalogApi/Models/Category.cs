using System.Collections.ObjectModel;

namespace CatalogApi.Models;

public class Category
{
    public Category()
    {
        Products = new Collection<Product>();
    }

    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public ICollection<Product> Products { get; set; }
}