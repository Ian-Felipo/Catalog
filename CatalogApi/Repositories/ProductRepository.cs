using CatalogApi.Data;
using CatalogApi.Interfaces;
using CatalogApi.Models;
using CatalogApi.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(CatalogApiDbContext catalogApiDbContext) : base(catalogApiDbContext)
    {
    }

    public PagedList<Product> GetProductsPagedList(ProductsParameters productsParameters)
    {
        IQueryable<Product> products = GetAll().AsQueryable();
        return PagedList<Product>.ToPagedList(products, productsParameters.PageNumber, productsParameters.PageSize);
    }
}