using AutoMapper;
using Courses.Dtos.ManualReviewDtos.Response;
using Courses.Models;

namespace Courses.Profiles.Converters;

public class UserAnswerToTasksListConverter : ITypeConverter<List<ManualReviewTaskUserAnswer>,
    List<CourseWithManualReviewTasksDto>>
{
    public List<CourseWithManualReviewTasksDto> Convert(List<ManualReviewTaskUserAnswer> source,
        List<CourseWithManualReviewTasksDto> destination, ResolutionContext context)
    {
        var coursesWithTasks = source
            .DistinctBy(taskUserAnswer => taskUserAnswer.CourseId)
            .Select(taskUserAnswer => context.Mapper.Map<CourseWithManualReviewTasksDto>(taskUserAnswer))
            .ToList();
        foreach (var courseWithTasks in coursesWithTasks)
        {
            courseWithTasks.TasksToCheck = new List<ManualReviewTaskInfoDto>();
            foreach (var taskUserAnswer in source.Where(taskUserAnswer =>
                         taskUserAnswer.CourseId == courseWithTasks.CourseId))
            {
                courseWithTasks.TasksToCheck.Add(context.Mapper.Map<ManualReviewTaskInfoDto>(taskUserAnswer));
            }
        }

        return coursesWithTasks;
    }
}
