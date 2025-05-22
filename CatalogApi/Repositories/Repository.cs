using System.Linq.Expressions;
using CatalogApi.Data;
using CatalogApi.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        return _catalogApiDbContext.Set<T>().AsNoTracking().ToList();
    }

    public T? Get(Expression<Func<T,bool>> predicado)
    {
        return _catalogApiDbContext.Set<T>().AsNoTracking().FirstOrDefault(predicado);
    }

    public T Post(T entity)
    {
        _catalogApiDbContext.Set<T>().Add(entity);
        return entity;
    }

    public T Put(T entity)
    {
        _catalogApiDbContext.Set<T>().Update(entity);
        return entity;
    }

    public T Delete(T entity)
    {
        _catalogApiDbContext.Set<T>().Remove(entity);
        return entity;
    }
}