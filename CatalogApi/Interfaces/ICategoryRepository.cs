using System.Runtime.CompilerServices;
using CatalogApi.Data;
using CatalogApi.Models;
using CatalogApi.Pagination;

namespace CatalogApi.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    PagedList<Category> GetCategories(CategoriesParameters categoriesParameters);
    IEnumerable<Category> GetCategoriesProducts();
    Category? GetCategoryProducts(int id);
    PagedList<Category> GetCategoriesProducts(CategoriesParameters productsParameters);
}