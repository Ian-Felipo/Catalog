using CatalogApi.Data;
using CatalogApi.Interfaces;
using CatalogApi.Models;
using CatalogApi.Parameters;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(CatalogApiDbContext catalogApiDbContext) : base(catalogApiDbContext)
    {
    }

    public IEnumerable<Product> GetProducts(ProductsParameters productsParameters)
    {
        return GetAll().Skip((productsParameters.PageNumber - 1) * productsParameters.PageSize).Take(productsParameters.PageSize).ToList();
    }
}