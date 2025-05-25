using CatalogApi.Models;
using CatalogApi.Parameters;

namespace CatalogApi.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    IEnumerable<Product> GetProducts(ProductsParameters productsParameters);
}