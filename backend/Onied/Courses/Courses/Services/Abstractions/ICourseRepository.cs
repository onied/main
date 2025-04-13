using Courses.Data.Models;
using Courses.Dtos.Catalog.Request;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Abstractions;

public interface ICourseRepository
{
    public Task<int> CountAsync();
    public Task<List<Course>> GetCoursesAsync(int? offset = null, int? limit = null);
    public Task<(List<Course> list, int count)> GetCoursesAsync(CatalogGetQueriesRequest catalogGetQueriesRequest);
    public Task<Course?> GetCourseAsync(int id);
    public Task<Course?> GetCourseWithBlocksAsync(int id);
    public Task<Course?> GetCourseWithUsersAsync(int id);
    public Task<Course?> GetCourseWithModeratorsAsync(int id);
    public Task<Course?> GetCourseWithUsersAndModeratorsAsync(int id);
    public Task<List<Course>> GetMostPopularCourses(int amount);
    public Task<List<Course>> GetRecommendedCourses(int amount);
    public Task<Course> AddCourseAsync(Course course);
    public Task AddModeratorAsync(int courseId, Guid studentId);
    public Task UpdateCourseAsync(Course course);
    public Task DeleteModeratorAsync(int courseId, Guid studentId);
    public Task RemoveAsyncById(int courseId);
}
