namespace OliveOilEshop.Repository;

public interface IUnitOfWork
{
    int Commit();

    void Rollback();

    Task<int> CommitAsync(CancellationToken cancellationToken);

    Task RollbackAsync();
}