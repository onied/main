using Courses.Models;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services;

public interface IUserRepository
{
    public Task<User?> GetUserAsync(Guid id);
    public Task<User?> GetUserWithCoursesAsync(Guid id);
    public Task SaveUserAsync(User user);
}