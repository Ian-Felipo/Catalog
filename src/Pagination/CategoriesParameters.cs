namespace CatalogApi.Pagination;

public class CategoriesParameters : QueryParameters
{
    public bool products { get; set; } = false;
}