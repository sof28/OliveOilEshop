using ModularCommerce.Database;

namespace ModularCommerce.Repository;

public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public int Commit()
    {
        return dbContext.SaveChanges();
    }

    public void Rollback()
    {
        dbContext.Dispose();
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RollbackAsync()
    {
        await dbContext.DisposeAsync();
    }
}
