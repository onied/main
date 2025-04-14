using Microsoft.EntityFrameworkCore.Storage;

namespace Purchases.Data.Abstractions;

public interface IPurchaseUnitOfWork
{
    ICourseRepository Courses { get; }
    IUserCourseInfoRepository UserCourseInfos { get; }
    IUserRepository Users { get; }
    IPurchaseRepository Purchases { get; }
    ISubscriptionRepository Subscriptions { get; }
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
