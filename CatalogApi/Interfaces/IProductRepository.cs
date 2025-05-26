using CatalogApi.Models;
using CatalogApi.Pagination;

namespace CatalogApi.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    PagedList<Product> GetProductsPagedList(ProductsParameters productsParameters);
}