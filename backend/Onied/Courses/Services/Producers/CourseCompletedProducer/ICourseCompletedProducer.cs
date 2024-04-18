using Courses.Models;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Producers.CourseCompletedProducer;

public interface ICourseCompletedProducer
{
    public Task PublishAsync(Course course, User user);
}
