using Microsoft.EntityFrameworkCore.Query;

namespace OliveOilEshop.Repository;

public interface IRepository<T>  where T : class
{
    public Task<IEnumerable<T>> GetAllAsync(CancellationToken token);
    public Task<T?> GetByIdAsync(long id, CancellationToken token);
    public Task<T> AddAsync(T entity, CancellationToken token);
    public Task UpdateAsync(T entity, CancellationToken token);
    public Task DeleteAsync(T entity, CancellationToken token);
    public Task RemoveRangeAsync(List<T> entities, CancellationToken token);
    public Task UpdateRangeAsync(List<T> entities, CancellationToken token);
}