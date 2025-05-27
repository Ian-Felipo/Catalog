using CatalogApi.Models;
using CatalogApi.Pagination;

namespace CatalogApi.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<PagedList<Product>> GetProductsPagedListAsync(ProductsParameters productsParameters);
    Task<PagedList<Product>> GetProductsPagedListFilterPriceAsync(ProductsFilterPrice productsFilterPrice);
}