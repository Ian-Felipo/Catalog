using CatalogApi.Models;

namespace CatalogApi.Interfaces;

public interface IProductRepository
{
    IEnumerable<Product> GetProducts();
    Product? GetProduct(int id);
    Product PostProduct(Product product);
    Product PutProduct(Product product);
    Product Delete(Product product);
}