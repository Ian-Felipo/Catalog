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

    public PagedList<Product> GetProductsPagedListFilterPrice(ProductsFilterPrice productsFilterPrice)
    {
        IQueryable<Product> products = GetAll().AsQueryable();

        if (productsFilterPrice.Price.HasValue && !string.IsNullOrEmpty(productsFilterPrice.Criterion))
        {
            if (productsFilterPrice.Criterion.Equals("maior", StringComparison.OrdinalIgnoreCase))
            {
                products = products.Where(product => product.Price > productsFilterPrice.Price);
            }
            else if (productsFilterPrice.Criterion.Equals("igual", StringComparison.OrdinalIgnoreCase))
            {
                products = products.Where(product => product.Price == productsFilterPrice.Price);
            }
            else if (productsFilterPrice.Criterion.Equals("menor", StringComparison.OrdinalIgnoreCase))
            {
                products = products.Where(product => product.Price < productsFilterPrice.Price);
            }
        }

        return PagedList<Product>.ToPagedList(products, productsFilterPrice.PageNumber, productsFilterPrice.PageSize);
    }
}