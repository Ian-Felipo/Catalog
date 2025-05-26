namespace CatalogApi.Pagination;

public abstract class QueryParameters
{
    private const int MaxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    private int _PageSize = MaxPageSize;
    public int PageSize
    {
        get
        {
            return _PageSize;
        }
        set
        {
            _PageSize = (value > MaxPageSize) ? MaxPageSize : value;
        } 
    }
}