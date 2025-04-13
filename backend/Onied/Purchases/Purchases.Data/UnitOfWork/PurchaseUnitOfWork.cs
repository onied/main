using Microsoft.EntityFrameworkCore.Storage;
using Purchases.Data.Abstractions;
using Purchases.Data.Repositories;

namespace Purchases.Data.UnitOfWork;

public class PurchaseUnitOfWork(AppDbContext dbContext) : IPurchaseUnitOfWork, IDisposable
{
    public ICourseRepository Courses { get; } = new CourseRepository(dbContext);
    public IUserCourseInfoRepository UserCourseInfos { get; } = new UserCourseInfoRepository(dbContext);
    public IUserRepository Users { get; } = new UserRepository(dbContext);
    public IPurchaseRepository Purchases { get; } = new PurchaseRepository(dbContext);
    public ISubscriptionRepository Subscriptions { get; } = new SubscriptionRepository(dbContext);
    private IDbContextTransaction? _currentTransaction;

    public async Task BeginTransactionAsync()
    {
        _currentTransaction = await dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction == null)
            throw new InvalidOperationException("The transaction has not been started");

        await _currentTransaction.CommitAsync();
        _currentTransaction = null;
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction == null)
            throw new InvalidOperationException("The transaction has not been started");

        await _currentTransaction.RollbackAsync();
        _currentTransaction = null;
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
    }
}
