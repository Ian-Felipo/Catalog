using CatalogApi.DTOs;
using CatalogApi.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CatalogApi.Mappers;

public static class ProductMapper
{
    public static Category? CategoryRequestToCategory(this CategoryRequest categoryRequest)
    {
        if (categoryRequest == null)
        {
            return null;
        }

        return new Category
        {
            Name = categoryRequest.Name,
            ImageUrl = categoryRequest.ImageUrl
        };
    }

    public static CategoryResponse? CategoryToCategoryResponse(this Category category)
    {
        if (category == null)
        {
            return null;
        }

        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            ImageUrl = category.ImageUrl,
            Products = category.Products,
        };
    }
}