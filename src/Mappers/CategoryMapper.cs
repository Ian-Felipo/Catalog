using CatalogApi.DTOs;
using CatalogApi.Models;

namespace CatalogApi.Mappers;

public static class CategoryMapper
{
    public static Category CategoryRequestToCategory(this CategoryRequest categoryRequest, int id = 0)
    {
        return new Category
        {
            Id = id,
            Name = categoryRequest.Name,
            ImageUrl = categoryRequest.ImageUrl
        };
    }

    public static CategoryResponse CategoryToCategoryResponse(this Category category)
    {
        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            ImageUrl = category.ImageUrl,
            Products = category.Products.Select(product => product.ProductToProductResponse())
        };
    }
}