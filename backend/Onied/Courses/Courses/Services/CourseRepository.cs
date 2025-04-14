using Courses.Data;
using Courses.Data.Models;
using Courses.Dtos.Catalog.Request;
using Courses.Services.Abstractions;
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

    public async Task<(List<Course> list, int count)> GetCoursesAsync(CatalogGetQueriesRequest catalogGetQueriesRequest)
    {
        var query = dbContext.Courses
            .Include(course => course.Author)
            .Include(course => course.Category)
            .AsNoTracking()
            .AsQueryable();

        if (catalogGetQueriesRequest.Category != null)
            query = query.Where(course => course.CategoryId == catalogGetQueriesRequest.Category.Value);
        if (catalogGetQueriesRequest.PriceFrom != null)
            query = query.Where(course => course.PriceRubles >= catalogGetQueriesRequest.PriceFrom.Value);
        if (catalogGetQueriesRequest.PriceTo != null)
            query = query.Where(course => course.PriceRubles <= catalogGetQueriesRequest.PriceTo.Value);
        if (catalogGetQueriesRequest.TimeFrom != null)
            query = query.Where(course => course.HoursCount >= catalogGetQueriesRequest.TimeFrom.Value);
        if (catalogGetQueriesRequest.TimeTo != null)
            query = query.Where(course => course.HoursCount <= catalogGetQueriesRequest.TimeTo.Value);
        if (catalogGetQueriesRequest.CertificatesOnly)
            query = query.Where(course => course.HasCertificates);
        if (catalogGetQueriesRequest.ActiveOnly)
            query = query.Where(course => !course.IsArchived);
        if (catalogGetQueriesRequest.Q.Length > 0)
            query = query.Where(course =>
                course.Title.ToLower().Contains(catalogGetQueriesRequest.Q.ToLower()) ||
                course.Description.ToLower().Contains(catalogGetQueriesRequest.Q.ToLower()));

        catalogGetQueriesRequest.Sort ??= "popular";

        switch (catalogGetQueriesRequest.Sort)
        {
            case "popular":
                query = query.OrderByDescending(course => course.Users.Count);
                break;
            case "new":
                query = query.OrderByDescending(course => course.CreatedDate);
                break;
            case "priceAsc":
                query = query.OrderBy(course => course.PriceRubles);
                break;
            case "priceDesc":
                query = query.OrderByDescending(course => course.PriceRubles);
                break;
        }

        return (await query.Skip(catalogGetQueriesRequest.Offset).Take(catalogGetQueriesRequest.ElementsOnPage).ToListAsync(),
            await query.CountAsync());
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

    public async Task<Course?> GetCourseWithUsersAsync(int id)
        => await dbContext.Courses
            .Include(c => c.Users)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Course?> GetCourseWithModeratorsAsync(int id)
        => await dbContext.Courses
            .Include(c => c.Moderators)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Course?> GetCourseWithUsersAndModeratorsAsync(int id)
        => await dbContext.Courses
            .Include(c => c.Moderators)
            .Include(c => c.Users)
            .Include(c => c.Author)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<List<Course>> GetMostPopularCourses(int amount)
        => await dbContext.Courses
            .Include(course => course.Author)
            .Include(course => course.Category)
            .Include(course => course.Users)
            .AsNoTracking()
            .OrderByDescending(course => course.Users.Count)
            .Take(amount)
            .ToListAsync();

    public async Task<List<Course>> GetRecommendedCourses(int amount)
        => await dbContext.Courses
            .Include(course => course.Author)
            .Include(course => course.Category)
            .AsNoTracking()
            .Where(course => course.IsGlowing)
            .OrderByDescending(course => course.Id)
            .Take(amount)
            .ToListAsync();


    public async Task<Course> AddCourseAsync(Course course)
    {
        var newCourse = await dbContext.Courses.AddAsync(course);
        await dbContext.SaveChangesAsync();
        return newCourse.Entity;
    }

    public async Task AddModeratorAsync(int courseId, Guid studentId)
    {
        var course = await dbContext.Courses
            .Include(c => c.Moderators)
            .FirstOrDefaultAsync(c => c.Id == courseId);
        if (course is not null)
        {
            var moderator = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == studentId);
            if (moderator is not null)
            {
                course.Moderators.Add(moderator);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    public async Task UpdateCourseAsync(Course course)
    {
        dbContext.Courses.Update(course);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteModeratorAsync(int courseId, Guid studentId)
    {
        var course = await dbContext.Courses
            .Include(c => c.Moderators)
            .FirstOrDefaultAsync(c => c.Id == courseId);
        if (course is not null)
        {
            var moderator = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == studentId);
            if (moderator is not null)
            {
                course.Moderators.Remove(moderator);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    public async Task RemoveAsyncById(int courseId)
    {
        var course = await dbContext.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
        if (course is not null)
        {
            dbContext.Courses.Remove(course);
            await dbContext.SaveChangesAsync();
        }
    }
}
