using Courses.Dtos;
using Courses.Models;
using Task = Courses.Models.Task;

namespace Courses.Services;

public class CheckTasksService : ICheckTasksService
{
    public UserTaskPoints? CheckTask(Task task, UserInputDto input)
    {
        if (!input.IsDone)
        {
            return new UserTaskPoints()
            {
                UserId = input.UserId,
                TaskId = input.TaskId,
                Points = 0
            };
        }

        return task.TaskType switch
        {
            TaskType.SingleAnswer or TaskType.MultipleAnswers => CheckTask((VariantsTask)task, input),
            TaskType.InputAnswer => CheckTask((InputTask)task, input),
            _ => null
        };
    }

    private UserTaskPoints CheckTask(VariantsTask task, UserInputDto input)
    {
        return new UserTaskPoints()
        {
            UserId = input.UserId,
            TaskId = input.TaskId,
            Points = task.Variants
                .Where(variant => variant.IsCorrect)
                .Select(variant => variant.Id)
                .SequenceEqual(input.VariantsIds!) ? task.MaxPoints : 0
        };
    }

    private UserTaskPoints CheckTask(InputTask task, UserInputDto input)
    {
        return new UserTaskPoints()
        {
            UserId = input.UserId,
            TaskId = input.TaskId,
            Points = task.Answers.Any(
                    answer => task.IsCaseSensitive
                        ? answer.Answer.Equals(input.Answer)
                        : answer.Answer.ToLower().Equals(input.Answer.ToLower())
                    ) ? task.MaxPoints : 0
        };
    }
}
