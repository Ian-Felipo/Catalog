using System.Runtime.CompilerServices;
using CatalogApi.Data;
using CatalogApi.Models;
using CatalogApi.Pagination;

namespace CatalogApi.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    IEnumerable<Category> GetAllWithProducts();
    Category? GetWithProducts(int id);
    PagedList<Category> GetPagedList(CategoriesParameters productsParameters);
    PagedList<Category> GetPagedListFilterName(CategoriesFilterName categoriesFilterName);
}