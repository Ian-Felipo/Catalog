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

    public IEnumerable<Category> GetCategoriesProducts()
    {
        List<Category> categories = _catalogApiDbContext.Categories.Include(category => category.Products).AsNoTracking().ToList();
        return categories;
    }

    public Category? GetCategoryProducts(int id)
    {
        Category? category = _catalogApiDbContext.Categories.Include(category => category.Products).AsNoTracking().FirstOrDefault(category => category.Id == id);
        return category;
    }
    
    public PagedList<Category> GetCategoriesPagedList(CategoriesParameters categoriesParameters)
    {
        IQueryable<Category> categories = GetAll().AsQueryable();
        return PagedList<Category>.ToPagedList(categories, categoriesParameters.PageNumber, categoriesParameters.PageSize); ;
    }
    
    public PagedList<Category> GetCategoriesProductsPagedList(CategoriesParameters categoriesParameters)
    {
        IQueryable<Category> categories = GetCategoriesProducts().AsQueryable();
        return PagedList<Category>.ToPagedList(categories, categoriesParameters.PageNumber, categoriesParameters.PageSize); ;
    }
}