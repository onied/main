using Courses.Data;
using Courses.Services.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Courses.Services;

public class CourseUnitOfWork(AppDbContext dbContext) : ICourseUnitOfWork
{
    public ICourseRepository Courses { get; } = new CourseRepository(dbContext);
    public IUserCourseInfoRepository UserCourseInfos { get; } = new UserCourseInfoRepository(dbContext);
    public IUserRepository Users { get; } = new UserRepository(dbContext);
    public ICategoryRepository Categories { get; } = new CategoryRepository(dbContext);
    public IBlockRepository Blocks { get; } = new BlockRepository(dbContext);
    public IModuleRepository Modules { get; } = new ModuleRepository(dbContext);
    public IBlockCompletedInfoRepository BlockCompletedInfos { get; } = new BlockCompletedInfoRepository(dbContext);
    public IManualReviewTaskUserAnswerRepository ManualReviewTaskUserAnswers { get; } = new ManualReviewTaskUserAnswerRepository(dbContext);
    public IUserTaskPointsRepository UserTaskPoints { get; } = new UserTaskPointsRepository(dbContext);
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
