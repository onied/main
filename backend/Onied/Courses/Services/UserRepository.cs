using Courses.Models;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetUserAsync(Guid id)
        => await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetUserWithCoursesAsync(Guid id)
        => await dbContext.Users
            .Include(u => u.Courses)
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task SaveUserAsync(User user)
    {
        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }
}
