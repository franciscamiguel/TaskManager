using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Contracts.Repositories.Generics;

namespace TaskManager.Persistence.Repositories.Generics;

public class RepositoryGeneric<T>(DataContext context) : IRepositoryGeneric<T>
    where T : class
{
    protected readonly DbSet<T> DbSet = context.Set<T>();

    public async Task Add(T entity)
    {
        await DbSet.AddAsync(entity);
    }

    public async Task AddRange(IEnumerable<T> entities)
    {
        await DbSet.AddRangeAsync(entities);
    }

    public void Remove(T item)
    {
        DbSet.Remove(item);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        DbSet.RemoveRange(entities);
    }

    public void Update(T entity)
    {
        DbSet.Update(entity);
    }

    public void UpdateRange(IEnumerable<T> entities)
    {
        DbSet.UpdateRange(entities);
    }

    public IQueryable<T> Get(params Expression<Func<T, object>>[] includeProperties)
    {
        var query = DbSet.AsNoTrackingWithIdentityResolution();

        if (includeProperties != null)
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        return query;
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<bool> SaveChangeAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}