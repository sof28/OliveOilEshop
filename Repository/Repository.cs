using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ModularCommerce.Database;

namespace ModularCommerce.Repository;

public class Repository<T>(ApplicationDbContext dbContext) : IRepository<T>
    where T : class
{
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken token)
    {
        return await _dbSet.ToListAsync(token);
    }

    public async Task<T?> GetByIdAsync(long id, CancellationToken token)
    {
        return await _dbSet.FindAsync([id, token], cancellationToken: token);
    }

    public async Task<T> AddAsync(T entity, CancellationToken token)
    {
        await _dbSet.AddAsync(entity, cancellationToken: token);
        return entity;
    }

    public Task UpdateAsync(T entity, CancellationToken token)
    {
        _dbSet.Update(entity);      
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity, CancellationToken token)
    {
        _dbSet.Remove(entity);      
        return Task.CompletedTask;    
    }

    public Task RemoveRangeAsync(List<T> entities, CancellationToken token)
    {
        _dbSet.RemoveRange(entities);
        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(List<T> entities, CancellationToken token)
    {
        _dbSet.UpdateRange(entities);
        return Task.CompletedTask;
    }
}
