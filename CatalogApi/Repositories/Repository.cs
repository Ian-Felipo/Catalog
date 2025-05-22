using CatalogApi.Data;
using CatalogApi.Interfaces;

namespace CatalogApi.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly CatalogApiDbContext _catalogApiDbContext;

    public Repository(CatalogApiDbContext catalogApiDbContext)
    {
        _catalogApiDbContext = catalogApiDbContext;
    }

    public IEnumerable<T> GetAll()
    {
        return _catalogApiDbContext.Set<T>().ToList();
    }

    public T? Get(Expression<Func<T, bool>> predicado)
    {
        return _catalogApiDbContext.Set<T>().FirstOrDefault(predicado);
    }

    public T Post(T entity)
    {
        _catalogApiDbContext.Set<T>().Add(entity);
        _catalogApiDbContext.SaveChanges();
        return entity;
    }

    public T Put(T entity)
    {
        _catalogApiDbContext.Set<T>().Update(entity);
        _catalogApiDbContext.SaveChanges();
        return entity;
    }

    public T Delete(T entity)
    {
        _catalogApiDbContext.Set<T>().Delete(entity);
        _catalogApiDbContext.SaveChanges();
        return entity;
    }
}