using Courses.Data.Models;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Producers.CourseCreatedProducer;

public interface ICourseCreatedProducer
{
    public Task PublishAsync(Course course);
}
