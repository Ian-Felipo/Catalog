using System.Threading.Tasks;
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

    public async Task<PagedList<Product>> GetProductsPagedListAsync(ProductsParameters productsParameters)
    {
        var products = await GetAllAsync();
        return PagedList<Product>.ToPagedList(products.AsQueryable(), productsParameters.PageNumber, productsParameters.PageSize);
    }

    public async Task<PagedList<Product>> GetProductsPagedListFilterPriceAsync(ProductsFilterPrice productsFilterPrice)
    {
        var products = await GetAllAsync();

        if (productsFilterPrice.Price.HasValue && !string.IsNullOrEmpty(productsFilterPrice.Criterion))
        {
            if (productsFilterPrice.Criterion.Equals("maior", StringComparison.OrdinalIgnoreCase))
            {
                products = products.AsQueryable().Where(product => product.Price > productsFilterPrice.Price);
            }
            else if (productsFilterPrice.Criterion.Equals("igual", StringComparison.OrdinalIgnoreCase))
            {
                products = products.AsQueryable().Where(product => product.Price == productsFilterPrice.Price);
            }
            else if (productsFilterPrice.Criterion.Equals("menor", StringComparison.OrdinalIgnoreCase))
            {
                products = products.AsQueryable().Where(product => product.Price < productsFilterPrice.Price);
            }
        }

        return PagedList<Product>.ToPagedList(products.AsQueryable(), productsFilterPrice.PageNumber, productsFilterPrice.PageSize);
    }
}