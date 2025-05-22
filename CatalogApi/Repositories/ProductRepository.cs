using CatalogApi.Data;
using CatalogApi.Interfaces;
using CatalogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(CatalogApiDbContext catalogApiDbContext) : base(catalogApiDbContext)
    {
    }
}