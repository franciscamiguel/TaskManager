using System.Linq.Expressions;

namespace TaskManager.Domain.Contracts.Repositories.Generics;

public interface IRepositoryGeneric<T> where T : class
{
    Task Add(T entity);

    Task AddRange(IEnumerable<T> entities);

    void Remove(T item);

    void RemoveRange(IEnumerable<T> entities);

    void Update(T entity);

    void UpdateRange(IEnumerable<T> entities);

    IQueryable<T> Get(params Expression<Func<T, object>>[] includeProperties);

    Task<T> GetByIdAsync(int id);

    Task<bool> SaveChangeAsync();
}