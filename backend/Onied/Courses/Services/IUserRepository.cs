using Courses.Models;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services;

public interface IUserRepository
{
    public Task<User?> GetUserAsync(Guid id);
    public Task<User?> GetUserWithCoursesAsync(Guid id);
    public Task<User?> GetUserWithTeachingCoursesAsync(Guid id);
    public Task<User?> GetUserWithModeratingCoursesAsync(Guid id);
    public Task AddUserAsync(User user);
    public Task UpdateUserAsync(User user);
    public Task<User?> GetUserWithModeratingAndTeachingCoursesAsync(Guid id);
}
