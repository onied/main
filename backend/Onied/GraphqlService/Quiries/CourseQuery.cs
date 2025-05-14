using Courses.Data;
using Courses.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphqlService.Quiries;

public class CourseQuery
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Course> GetCourses(AppDbContext dbContext)
        => dbContext.Courses;

    public async Task<Course> GetCourseById(
        int id,
        AppDbContext dbContext,
        [Service] IHttpContextAccessor contextAccessor)
    {
        var userId = contextAccessor.HttpContext!.Request.Headers["X-User-Id"].FirstOrDefault();
        if (userId is null)
        {
            throw new GraphQLException("Unauthorized access");
        }
        var course = await dbContext.Courses
            .Include(course => course.Modules)
            .Include(course => course.Users)
            .Include(course => course.Author)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (course is null)
        {
            throw new GraphQLException("Course not found");
        }
        var student = course.Users.FirstOrDefault(x => x.Id.ToString() == userId);
        var moderator = course.Moderators.FirstOrDefault(x => x.Id.ToString() == userId);
        var isAuthor = course.Author?.Id.ToString() == userId;
        if (student is null && moderator is null && !isAuthor)
        {
            throw new GraphQLException("Forbidden access");
        }

        return course;
    }
}
