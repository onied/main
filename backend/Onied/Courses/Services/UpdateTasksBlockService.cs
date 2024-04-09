using Courses.Dtos;
using Courses.Models;
using Courses.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Task = Courses.Models.Task;

namespace Courses.Services;

public class UpdateTasksBlockService(
    AppDbContext dbContext,
    IBlockRepository blockRepository
    ) : IUpdateTasksBlockService
{
    public async Task<TasksBlock> UpdateTasksBlock(EditTasksBlockDto tasksBlockDto)
    {
        dbContext.Tasks.RemoveRange(dbContext.Tasks.Where(t => t.TasksBlockId == tasksBlockDto.Id));
        await dbContext.SaveChangesAsync();
        foreach (var updatedTask in tasksBlockDto.Tasks)
        {
            var task = new Task
            {
                TasksBlockId = tasksBlockDto.Id,
                TaskType = updatedTask.TaskType,
                Title = updatedTask.Title,
                MaxPoints = updatedTask.MaxPoints
            };
            switch (updatedTask.TaskType)
            {
                case TaskType.SingleAnswer:
                    var variantsTask1 = new VariantsTask
                    {
                        TasksBlockId = task.TasksBlockId,
                        TaskType = task.TaskType,
                        Title = task.Title,
                        MaxPoints = task.MaxPoints
                    };
                    await dbContext.VariantsTasks.AddAsync(variantsTask1);
                    await dbContext.SaveChangesAsync();
                    foreach (var addedVariant in updatedTask.Variants)
                    {
                        await dbContext.TaskVariants.AddAsync(new TaskVariant
                        {
                            TaskId = variantsTask1.Id,
                            Description = addedVariant.Description ?? "",
                            IsCorrect = addedVariant.IsCorrect ?? false
                        });
                    }
                    break;
                case TaskType.MultipleAnswers:
                    var variantsTask2 = new VariantsTask
                    {
                        TasksBlockId = task.TasksBlockId,
                        TaskType = task.TaskType,
                        Title = task.Title,
                        MaxPoints = task.MaxPoints
                    };
                    await dbContext.VariantsTasks.AddAsync(variantsTask2);
                    await dbContext.SaveChangesAsync();
                    foreach (var addedVariant in updatedTask.Variants)
                    {
                        await dbContext.TaskVariants.AddAsync(new TaskVariant
                        {
                            TaskId = variantsTask2.Id,
                            Description = addedVariant.Description ?? "",
                            IsCorrect = addedVariant.IsCorrect ?? false
                        });
                    }
                    break;
                case TaskType.InputAnswer:
                    var answersTask = new InputTask
                    {
                        TasksBlockId = task.TasksBlockId,
                        TaskType = task.TaskType,
                        Title = task.Title,
                        MaxPoints = task.MaxPoints,
                        IsNumber = updatedTask.IsNumber ?? false,
                        Accuracy = updatedTask.Accuracy,
                        IsCaseSensitive = updatedTask.IsCaseSensitive ?? false
                    };
                    await dbContext.InputTasks.AddAsync(answersTask);
                    await dbContext.SaveChangesAsync();
                    foreach (var addedAnswer in updatedTask.Answers)
                    {
                        await dbContext.TaskTextInputAnswers.AddAsync(new TaskTextInputAnswer
                        {
                            TaskId = answersTask.Id,
                            Answer = addedAnswer.Answer ?? ""
                        });
                    }
                    break;
                default:
                    await dbContext.Tasks.AddAsync(task);
                    await dbContext.SaveChangesAsync();
                    break;
            }
        }
        await dbContext.SaveChangesAsync();

        return await blockRepository.GetTasksBlock(tasksBlockDto.Id, true, true);
    }
}
