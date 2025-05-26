using System.Runtime.CompilerServices;
using CatalogApi.Data;
using CatalogApi.Models;
using CatalogApi.Pagination;

namespace CatalogApi.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    IEnumerable<Category> GetCategoriesProducts();
    Category? GetCategoryProducts(int id);
    PagedList<Category> GetCategoriesPagedList(CategoriesParameters categoriesParameters);
    PagedList<Category> GetCategoriesProductsPagedList(CategoriesParameters productsParameters);
}