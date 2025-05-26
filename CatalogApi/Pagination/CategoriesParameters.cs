namespace CatalogApi.Pagination;

public class CategoriesParameters : QueryStringParameters
{
    public bool products { get; set; } = false;
}