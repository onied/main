namespace Courses.Models.Users;

public class Author : User
{

    public ICollection<Course> TeachingCourses { get; } = new List<Course>();
}
