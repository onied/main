namespace Courses.Services.Abstractions;

public interface ICourseUnitOfWork
{
    ICourseRepository Courses { get; }
    IUserCourseInfoRepository UserCourseInfos { get; }
    IUserRepository Users { get; }
    ICategoryRepository Categories { get; }
    IBlockRepository Blocks { get; }
    IModuleRepository Modules { get; }
    IBlockCompletedInfoRepository BlockCompletedInfos { get; }
    IManualReviewTaskUserAnswerRepository ManualReviewTaskUserAnswers { get; }
    IUserTaskPointsRepository UserTaskPoints { get; }
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
