using AutoMapper;
using CatalogApi.AutoMappers;
using CatalogApi.Data;
using CatalogApi.Interfaces;
using CatalogApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CatalogApiUnitTests.UnitTests;

public class ProductsControllerUnitTests
{
    public readonly IUnitOfWork unitOfWork;
    public readonly IMapper mapper;
    private readonly static DbContextOptions<CatalogApiDbContext> dbContextOptions;
    private readonly static string connectionString = new ConfigurationBuilder().AddUserSecrets("e6b54ad8-5553-489a-9092-b8901ecc18cc").Build()["ConnectionStrings:MySql"]!;
    
    static ProductsControllerUnitTests()
    {
        dbContextOptions = new DbContextOptionsBuilder<CatalogApiDbContext>().UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).Options;
    }

    public ProductsControllerUnitTests()
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProductMapperProfile());
            }
        );
        mapper = mapperConfiguration.CreateMapper();
        var context = new CatalogApiDbContext(dbContextOptions);
        unitOfWork = new UnitOfWork(context);
    }
}