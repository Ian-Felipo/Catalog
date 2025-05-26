namespace CatalogApi.Pagination;

public class ProductsFilterPrice : ProductsParameters
{
    public decimal? Price { get; set; } 
    public string? Criterion { get; set; }  
}