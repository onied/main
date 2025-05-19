namespace GraphqlService.Dtos.Course.Response;

public class CourseResponse : Courses.Data.Models.Course
{
    public bool IsOwned { get; set; }
}
