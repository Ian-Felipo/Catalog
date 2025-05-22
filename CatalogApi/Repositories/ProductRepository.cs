using CatalogApi.Data;
using CatalogApi.Interfaces;
using CatalogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.ProductRepository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly CatalogApiDbContext _catalogApiDbContext;

    public ProductRepository(CatalogApiDbContext catalogApiDbContext)
    {
        _catalogApiDbContext = catalogApiDbContext;
    }

    public IEnumerable<Product> GetProducts()
    {
        List<Product> products = _catalogApiDbContext.Products.AsNoTracking().ToList();
        return products;
    }

    public Product? GetProduct(int id)
    {
        Product? product = _catalogApiDbContext.Products.AsNoTracking().FirstOrDefault(product => product.Id == id);
        return product;
    }

    public Product PostProduct(Product product)
    {
        _catalogApiDbContext.Entry(product).State = EntityState.Modified;
        _catalogApiDbContext.SaveChanges();
        return product;
    }

    public Product PutProduct(Product product)
    {
        _catalogApiDbContext.Products.Add(product);
        _catalogApiDbContext.SaveChanges();
        return product;
    }

    public Product Delete(Product product)
    {
        _catalogApiDbContext.Products.Remove(product);
        _catalogApiDbContext.SaveChanges();
        return product;
    }
}