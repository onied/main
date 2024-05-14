using AutoMapper;
using Courses.Dtos.ManualReview.Response;
using Courses.Models;

namespace Courses.Profiles.Converters;

public class UserAnswerToTasksListConverter : ITypeConverter<List<ManualReviewTaskUserAnswer>,
    List<CourseWithManualReviewTasksResponse>>
{
    public List<CourseWithManualReviewTasksResponse> Convert(List<ManualReviewTaskUserAnswer> source,
        List<CourseWithManualReviewTasksResponse> destination, ResolutionContext context)
    {
        var coursesWithTasks = source
            .DistinctBy(taskUserAnswer => taskUserAnswer.CourseId)
            .Select(taskUserAnswer => context.Mapper.Map<CourseWithManualReviewTasksResponse>(taskUserAnswer))
            .ToList();
        foreach (var courseWithTasks in coursesWithTasks)
        {
            courseWithTasks.TasksToCheck = new List<ManualReviewTaskInfoResponse>();
            foreach (var taskUserAnswer in source.Where(taskUserAnswer =>
                         taskUserAnswer.CourseId == courseWithTasks.CourseId))
            {
                courseWithTasks.TasksToCheck.Add(context.Mapper.Map<ManualReviewTaskInfoResponse>(taskUserAnswer));
            }
        }

        return coursesWithTasks;
    }
}
