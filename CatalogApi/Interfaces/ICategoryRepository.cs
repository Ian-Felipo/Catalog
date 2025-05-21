using System.Runtime.CompilerServices;
using CatalogApi.Data;
using CatalogApi.Models;

namespace CatalogApi.Interfaces;

public interface ICategoryRepository
{
    IEnumerable<Category> GetCategories();
    Category? GetCategory(int id);
    IEnumerable<Category> GetCategoriesProducts();
    Category? GetCategoryProducts(int id);
    Category PostCategory(Category category);
    Category PutCategory(Category category);
    Category DeleteCategory(Category category);
}