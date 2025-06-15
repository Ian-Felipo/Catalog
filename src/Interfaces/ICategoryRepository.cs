using System.Runtime.CompilerServices;
using CatalogApi.Data;
using CatalogApi.Models;
using CatalogApi.Pagination;

namespace CatalogApi.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetAllWithProductsAsync();
    Task<Category?> GetWithProductsAsync(int id);
    Task<PagedList<Category>> GetPagedListAsync(CategoriesParameters productsParameters);
    Task<PagedList<Category>> GetPagedListFilterNameAsync(CategoriesFilterName categoriesFilterName);
}