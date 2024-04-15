using Courses.Models;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Abstractions;

public interface ICourseRepository
{
    public Task<int> CountAsync();
    public Task<List<Course>> GetCoursesAsync(int? offset = null, int? limit = null);
    public Task<Course?> GetCourseAsync(int id);
    public Task<Course?> GetCourseWithBlocksAsync(int id);
    public Task<Course> AddCourseAsync(Course course);
    public Task UpdateCourseAsync(Course course);
}
