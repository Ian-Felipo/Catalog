namespace CatalogApi.Interfaces;

public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    T? Get(Expression<Func<T,bool>> predicado);
    T Post(T entity);
    T Put(T entity);
    T Delete(T entity);
}