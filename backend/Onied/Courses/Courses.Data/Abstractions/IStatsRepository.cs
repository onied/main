namespace Courses.Data.Abstractions;

public interface IStatsRepository
{
    Task CreateTablesIfNotExistsAsync();
    Task<int> GetCourseLikesAsync(int courseId);
    Task LikeCourseAsync(int courseId, Guid userId);
    Task UnlikeCourseAsync(int courseId, Guid userId);
    Task<bool> IsCourseLikedAsync(int courseId, Guid userId);
}
