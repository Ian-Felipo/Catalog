using CatalogApi.Data;
using CatalogApi.Interfaces;

namespace CatalogApi.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private ICategoryRepository? _categoryRepository;
    private IProductRepository? _productRepository;
    public CatalogApiDbContext _catalogApiDbContext;

    public UnitOfWork(CatalogApiDbContext catalogApiDbContext)
    {
        _catalogApiDbContext = catalogApiDbContext;
    }

    public ICategoryRepository CategoryRepository
    {
        get
        {
            return _categoryRepository = _categoryRepository ?? new CategoryRepository(_catalogApiDbContext);
        }
    }

    public IProductRepository ProductRepository
    {
        get
        {
            return _productRepository = _productRepository ?? new ProductRepository(_catalogApiDbContext);
        }
    }

    public async Task CommitAsync()
    {
        await _catalogApiDbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _catalogApiDbContext.Dispose();
    }
}