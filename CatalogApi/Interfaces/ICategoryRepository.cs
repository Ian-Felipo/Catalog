using System.Runtime.CompilerServices;
using CatalogApi.Data;
using CatalogApi.Models;

namespace CatalogApi.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    IEnumerable<Category> GetCategoriesProducts();
    Category? GetCategoryProducts(int id);
}