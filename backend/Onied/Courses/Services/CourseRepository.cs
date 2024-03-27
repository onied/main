using Courses.Models;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services;

public class CourseRepository(AppDbContext dbContext) : ICourseRepository
{
    public Task<int> CountAsync()
        => dbContext.Courses.CountAsync();

    public async Task<List<Course>> GetCoursesAsync(int? offset = null, int? limit = null)
    {
        var query = dbContext.Courses
            .Include(course => course.Author)
            .Include(course => course.Category)
            .AsNoTracking()
            .AsQueryable();

        if (offset is not null)
        {
            query = query.Skip(offset.Value);
        }
        if (limit is not null)
        {
            query = query.Take(limit.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Course?> GetCourseAsync(int id)
    => await dbContext.Courses
        .Include(course => course.Modules)
        .Include(course => course.Author)
        .Include(course => course.Category)
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Course?> GetCourseWithBlocksAsync(int id)
        => await dbContext.Courses
            .Include(course => course.Modules)
                .ThenInclude(module => module.Blocks)
            .Include(course => course.Author)
            .Include(course => course.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task AddCourseAsync(Course course)
    {
        await dbContext.Courses.AddAsync(course);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateCourseAsync(Course course)
    {
        dbContext.Courses.Update(course);
        await dbContext.SaveChangesAsync();
    }
}
