using CatalogApi.Data;
using CatalogApi.Interfaces;
using CatalogApi.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;

namespace CatalogApi.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly CatalogApiDbContext _catalogApiDbContext;

    public CategoryRepository(CatalogApiDbContext catalogApiDbContext)
    {
        _catalogApiDbContext = catalogApiDbContext;
    }

    public IEnumerable<Category> GetCategories()
    {
        List<Category> categories = _catalogApiDbContext.Categories.AsNoTracking().ToList();
        return categories;
    }

    public Category? GetCategory(int id)
    {
        Category? category = _catalogApiDbContext.Categories.AsNoTracking().FirstOrDefault(category => category.Id == id);
        return category;
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

    public Category PostCategory(Category category)
    {
        _catalogApiDbContext.Categories.Add(category);
        _catalogApiDbContext.SaveChanges();
        return category;         
    }

    public Category PutCategory(Category category)
    {
        _catalogApiDbContext.Entry(category).State = EntityState.Modified;
        _catalogApiDbContext.SaveChanges();
        return category;
    }

    public Category DeleteCategory(Category category)
    {
        _catalogApiDbContext.Categories.Remove(category);
        _catalogApiDbContext.SaveChanges();
        return category;
    }
}