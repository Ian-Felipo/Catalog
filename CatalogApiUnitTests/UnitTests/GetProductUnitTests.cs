using CatalogApi.Controllers;
using CatalogApi.DTOs;
using CatalogApi.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

namespace CatalogApiUnitTests.UnitTests;

public class GetProductUnitTests : IClassFixture<ProductsControllerUnitTests>
{
      private readonly ProductsController productsController;

    public GetProductUnitTests(ProductsControllerUnitTests productsControllerUnitTests)
    {
        productsController = new ProductsController(productsControllerUnitTests.unitOfWork, productsControllerUnitTests.mapper);
    }

    [Fact]
    public async Task GetProductById_OkResult()
    {
        // Arrange

        int id = 1;

        // Act

        var product = await productsController.Get(id);

        Console.WriteLine(product.Value);
        // Assert Xunit

        var result = Assert.IsType<OkObjectResult>(product.Result);
        Assert.Equal(200, result.StatusCode);

        var value = Assert.IsType<ProductResponse>(product.Value);

        // Assert Fluent

        product.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(6)]
    public async Task GetProductId_OkResult(int id)
    {
        // Act

        var product = await productsController.Get(id);

        // Assert Xunit

        var result = Assert.IsType<OkObjectResult>(product.Result);
        Assert.Equal(200, result.StatusCode);
    }
}