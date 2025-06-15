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

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _catalogApiDbContext.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetAsync(Expression<Func<T,bool>> predicado)
    {
        return await _catalogApiDbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicado);
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