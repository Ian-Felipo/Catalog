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

    public IEnumerable<Category> GetAllWithProducts()
    {
        List<Category> categories = _catalogApiDbContext.Categories.Include(category => category.Products).AsNoTracking().ToList();
        return categories;
    }

    public Category? GetWithProducts(int id)
    {
        Category? category = _catalogApiDbContext.Categories.Include(category => category.Products).AsNoTracking().FirstOrDefault(category => category.Id == id);
        return category;
    }

    public PagedList<Category> GetPagedList(CategoriesParameters categoriesParameters)
    {
        IQueryable<Category> categories = categoriesParameters.products ? GetAllWithProducts().AsQueryable() : GetAll().AsQueryable();
        return PagedList<Category>.ToPagedList(categories, categoriesParameters.PageNumber, categoriesParameters.PageSize); 
    }

    public PagedList<Category> GetPagedListFilterName(CategoriesFilterName categoriesFilterName)
    {
        IQueryable<Category> categories = categoriesFilterName.products ? GetAllWithProducts().AsQueryable() : GetAll().AsQueryable();

        if (!string.IsNullOrEmpty(categoriesFilterName.Name))
        {
            categories = categories.Where(category => category.Name.Contains(categoriesFilterName.Name));
        }

        return PagedList<Category>.ToPagedList(categories, categoriesFilterName.PageNumber, categoriesFilterName.PageSize);
    }
}