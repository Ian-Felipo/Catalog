using System.Linq.Expressions;

namespace CatalogApi.Interfaces;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetAsync(Expression<Func<T,bool>> predicado);
    T Post(T entity);
    T Put(T entity);
    T Delete(T entity);
}