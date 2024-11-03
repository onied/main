using MassTransit.Data.Messages;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Producers.CourseCompletedProducer;

public interface ICourseCompletedProducer
{
    public Task PublishAsync(CourseCompleted courseCompleted);
}
