using CatalogApi.Models;
using CatalogApi.Pagination;
using CatalogApi.Parameters;

namespace CatalogApi.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    PagedList<Product> GetProducts(ProductsParameters productsParameters);
}