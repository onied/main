using System.Linq.Expressions;
using Courses.Data;
using Courses.Data.Models;
using Courses.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<List<User>> GetUsersWithConditionAsync(Expression<Func<User, bool>>? condition = null)
    {
        var query = dbContext.Users.AsNoTracking().AsQueryable();

        if (condition is null) return await query.ToListAsync();

        return await query.Where(condition).ToListAsync();
    }

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

    public async Task<User?> GetUserWithTeachingCoursesAsync(Guid id)
        => await dbContext.Users
            .Include(u => u.TeachingCourses)
            .ThenInclude(c => c.Category)
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetUserWithModeratingCoursesAsync(Guid id)
        => await dbContext.Users
            .Include(u => u.ModeratingCourses)
            .ThenInclude(c => c.Author)
            .Include(u => u.ModeratingCourses)
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

    public async Task<User?> GetUserWithModeratingAndTeachingCoursesAsync(Guid id)
    {
        return await dbContext.Users
            .Include(u => u.TeachingCourses)
            .Include(u => u.ModeratingCourses)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
}
