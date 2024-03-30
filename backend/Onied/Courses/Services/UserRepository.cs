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
            .ThenInclude(c => c.Author)
            .Include(u => u.Courses)
            .ThenInclude(c => c.Category)
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task AddUserAsync(User user)
    {
        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        dbContext.Update(user);
        await dbContext.SaveChangesAsync();
    }
}
