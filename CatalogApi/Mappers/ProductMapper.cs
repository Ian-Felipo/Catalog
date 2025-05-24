using CatalogApi.DTOs;
using CatalogApi.Models;

namespace CatalogApi.Mappers;

public static class ProductMapper
{
    public static Product ProductRequestToProduct(this ProductRequest productRequest, int id = 0)
    {
        return new Product
        {
            Id = id,
            Name = productRequest.Name,
            Description = productRequest.Description,
            Price = productRequest.Price,
            ImageUrl = productRequest.ImageUrl,
            Stock = productRequest.Stock,
            RegistrationDate = productRequest.RegistrationDate,
            CategoryId = productRequest.CategoryId
        };
    }

    public static ProductResponse ProductToProductResponse(this Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Stock = product.Stock,
            RegistrationDate = product.RegistrationDate,
            CategoryId = product.CategoryId
        };
    }
}