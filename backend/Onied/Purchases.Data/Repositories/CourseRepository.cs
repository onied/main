using Microsoft.EntityFrameworkCore;
using Purchases.Data.Abstractions;
using Purchases.Data.Models;

namespace Purchases.Data.Repositories;

public class CourseRepository(AppDbContext dbContext) : ICourseRepository
{
    public async Task<Course?> GetAsync(int id)
        => await dbContext.Courses
            .Include(c => c.Author)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task AddAsync(Course course)
    {
        await dbContext.Courses.AddAsync(course);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Course course)
    {
        dbContext.Courses.Update(course);
        await dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(Course course)
    {
        dbContext.Courses.Remove(course);
        await dbContext.SaveChangesAsync();
    }
}
