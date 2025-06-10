namespace CatalogApiUnitTests.UnitTests;

public class ProductsControllerUnitTests
{
    public IUnitOfWork unitOfWork;
    public IMapper mapper;
    public static DbContextOptions<CatalogApiDbContext> dbContextOptions { get; }

    static ProdutosUnitTestController()
    {
        dbContextOptions = new DbContextOptionsBuilder<CatalogApiDbContext>().UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).Options;
    }

    public ProdutosUnitTestController()
    {
        var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProdutoDTOMappingProfile());
            }
        );
        mapper = config.CreateMapper();
        var context = new AppDbContext(dbContextOptions);
        repository = new UnitOfWork(context);
    }
}