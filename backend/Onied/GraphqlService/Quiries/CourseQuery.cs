using Courses.Data;
using Courses.Data.Models;

namespace GraphqlService.Quiries;

public class CourseQuery
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Course> GetCourses(AppDbContext dbContext)
        => dbContext.Courses;

    public Course GetCourseById(
        int id,
        [Service] IHttpContextAccessor contextAccessor,
        AppDbContext dbContext)
    {
        var userId = contextAccessor.HttpContext!.Request.Headers["X-User-Id"].FirstOrDefault();
        var course = dbContext.Courses.FirstOrDefault(x => x.Id == id);

        if (userId == null)
            throw new GraphQLException("Доступ запрещен");

        return course;
    }
}
