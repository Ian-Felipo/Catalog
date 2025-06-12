using CatalogApi.Controllers;
using CatalogApi.Pagination;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogApiUnitTests.UnitTests;

public class GetProductUnitTests : IClassFixture<ProductsControllerUnitTests>
{
    private readonly ProductsController productsController;

    public GetProductUnitTests(ProductsControllerUnitTests productsControllerUnitTests)
    {
        productsController = new ProductsController(productsControllerUnitTests.unitOfWork, productsControllerUnitTests.mapper);
        productsController.ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext() };
    }

    [Fact]
    public async Task GetProductById_OkResult_Fact()
    {
        // Arrange

        int id = 1;

        // Act

        var product = await productsController.Get(id);

        // Assert Xunit

        var result = Assert.IsType<OkObjectResult>(product.Result);
        Assert.Equal(200, result.StatusCode);

        // Assert Fluent

        product.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(6)]
    public async Task GetProductById_OkResult_Theory(int id)
    {
        // Act

        var product = await productsController.Get(id);

        // Assert Xunit

        var result = Assert.IsType<OkObjectResult>(product.Result);
        Assert.Equal(200, result.StatusCode);

        // Assert Fluent

        product.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetProductById_NotFoundResult_Fact()
    {
        // Arrange

        int id = 111;

        // Act

        var product = await productsController.Get(id);

        // Assert Xunit

        var result = Assert.IsType<NotFoundResult>(product.Result);
        Assert.Equal(404, result.StatusCode);

        // Assert Fluent


    }

    [Theory]
    [InlineData(111)]
    [InlineData(222)]
    [InlineData(333)]
    [InlineData(444)]
    public async Task GetProductById_NotFoundResult_Theory(int id)
    {
        // Act

        var product = await productsController.Get(id);

        // Assert Xunit

        var result = Assert.IsType<NotFoundResult>(product.Result);
        Assert.Equal(404, result.StatusCode);

        // Assert Fluent

        product.Result.Should().BeOfType<NotFoundResult>().Which.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetProducts_OkResult_Fact()
    {
        // Arrange

        var productsParameters = new ProductsParameters();

        // Act

        var products = await productsController.Get(productsParameters);

        // Assert Xunit

        var result = Assert.IsType<OkObjectResult>(products.Result);
        Assert.Equal(200, result.StatusCode);

        // Assert Fluent

        products.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 3)]
    [InlineData(3, 3)]
    [InlineData(1, 6)]
    [InlineData(2, 6)]
    [InlineData(3, 6)]
    public async Task GetProducts_OkResult_Theory(int pageNumber, int pageSize)
    {
        // Arrange

        var productsParameters = new ProductsParameters()
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        // Act

        var products = await productsController.Get(productsParameters);

        // Assert Xunit

        var result = Assert.IsType<OkObjectResult>(products.Result);
        Assert.Equal(200, result.StatusCode);

        // Assert Fluent

        products.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
    }
}