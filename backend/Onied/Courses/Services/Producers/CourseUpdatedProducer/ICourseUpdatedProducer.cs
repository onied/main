using Courses.Models;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Producers.CourseUpdatedProducer;

public interface ICourseUpdatedProducer
{
    public Task PublishAsync(Course course);
}
