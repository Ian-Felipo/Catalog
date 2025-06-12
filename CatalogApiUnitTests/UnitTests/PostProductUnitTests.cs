using CatalogApi.Controllers;
using CatalogApi.DTOs;
using CatalogApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CatalogApiUnitTests.UnitTests;

public class PostProductUnitTests : IClassFixture<ProductsControllerUnitTests>
{
    private readonly ProductsController productsController;

    PostProductUnitTests(ProductsControllerUnitTests productsControllerUnitTests)
    {
        productsController = new ProductsController(productsControllerUnitTests.unitOfWork, productsControllerUnitTests.mapper);
    }

    [Fact]
    public async Task PostProduct_CreatedResult_Fact()
    {
        // Arrange

        ProductRequest productRequest = new ProductRequest
        {
            Name = "Produto",
            Price = 1000.00M,
            ImageUrl = "Produto.jpg"
        };

        // Act

        var product = await productsController.Post(productRequest);

        // Assert Xunit

        var result = Assert.IsType<CreatedAtRouteResult>(product.Result);
    }
}