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
}
