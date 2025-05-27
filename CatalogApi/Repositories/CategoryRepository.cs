using System.Threading.Tasks;
using CatalogApi.Data;
using CatalogApi.Interfaces;
using CatalogApi.Models;
using CatalogApi.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(CatalogApiDbContext catalogApiDbContext) : base(catalogApiDbContext)
    {
    }

    public async Task<IEnumerable<Category>> GetAllWithProductsAsync()
    {
        List<Category> categories = await _catalogApiDbContext.Categories.Include(category => category.Products).AsNoTracking().ToListAsync();
        return categories;
    }

    public async Task<Category?> GetWithProductsAsync(int id)
    {
        Category? category = await _catalogApiDbContext.Categories.Include(category => category.Products).AsNoTracking().FirstOrDefaultAsync(category => category.Id == id);
        return category;
    }

    public async Task<PagedList<Category>> GetPagedListAsync(CategoriesParameters categoriesParameters)
    {
        var categories = categoriesParameters.products ? await GetAllWithProductsAsync() : await GetAllAsync();
        return PagedList<Category>.ToPagedList(categories.AsQueryable(), categoriesParameters.PageNumber, categoriesParameters.PageSize); 
    }

    public async Task<PagedList<Category>> GetPagedListFilterNameAsync(CategoriesFilterName categoriesFilterName)
    {
        var categories = categoriesFilterName.products ? await GetAllWithProductsAsync() : await GetAllAsync();

        if (!string.IsNullOrEmpty(categoriesFilterName.Name))
        {
            categories = categories.AsQueryable().Where(category => category.Name.Contains(categoriesFilterName.Name));
        }

        return PagedList<Category>.ToPagedList(categories.AsQueryable(), categoriesFilterName.PageNumber, categoriesFilterName.PageSize);
    }
}