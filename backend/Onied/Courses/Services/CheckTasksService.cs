using Courses.Dtos;
using Courses.Dtos.CheckTasks.Request;
using Courses.Models;
using Courses.Services.Abstractions;
using Task = Courses.Models.Task;

namespace Courses.Services;

public class CheckTasksService : ICheckTasksService
{
    public UserTaskPoints CheckTask(Task task, UserInputRequest input)
    {
        if (!input.IsDone)
        {
            return new UserTaskPoints()
            {
                TaskId = input.TaskId,
                Points = 0,
                Checked = true,
            };
        }

        return task.TaskType switch
        {
            TaskType.SingleAnswer or TaskType.MultipleAnswers => CheckTask((VariantsTask)task, input),
            TaskType.InputAnswer => CheckTask((InputTask)task, input),
            TaskType.ManualReview => new ManualReviewTaskUserAnswer
            {
                TaskId = input.TaskId,
                Points = 0,
                Checked = false,
                Content = input.Text!,
                ManualReviewTaskUserAnswerId = Guid.NewGuid()
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private UserTaskPoints CheckTask(VariantsTask task, UserInputRequest input)
    {
        return new UserTaskPoints()
        {
            TaskId = input.TaskId,
            Points = task.Variants
                .Where(variant => variant.IsCorrect)
                .Select(variant => variant.Id).OrderBy(vid => vid)
                .SequenceEqual(input.VariantsIds!.OrderBy(vid => vid))
                ? task.MaxPoints
                : 0,
            Checked = true,
        };
    }

    private UserTaskPoints CheckTask(InputTask task, UserInputRequest input)
    {
        return new UserTaskPoints()
        {
            TaskId = input.TaskId,
            Points = task.Answers.Any(
                answer => task.IsCaseSensitive
                    ? answer.Answer.Equals(input.Answer)
                    : answer.Answer.ToLower().Equals(input.Answer!.ToLower())
            )
                ? task.MaxPoints
                : 0,
            Checked = true,
        };
    }
}
