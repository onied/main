using System.Collections.Immutable;
using Courses.Models;
using Courses.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services;

public class UserTaskPointsRepository(AppDbContext dbContext)
    : IUserTaskPointsRepository
{
    public async Task<UserTaskPoints?> GetConcreteUserTaskPoints(Guid userId, int taskId)
        => await dbContext.UserTaskPoints
            .AsNoTracking()
            .FirstOrDefaultAsync(tp => tp.UserId == userId && tp.TaskId == taskId);

    public async Task<List<UserTaskPoints>> GetUserTaskPointsByUserAndCourse(Guid userId, int courseId)
        => await dbContext.UserTaskPoints
            .Where(tp => tp.UserId == userId && tp.CourseId == courseId)
            .AsNoTracking()
            .ToListAsync();

    public async Task<List<UserTaskPoints>> GetUserTaskPointsByUserAndBlock(Guid userId, int courseId, int blockId)
        => await dbContext.UserTaskPoints
            .Include(tp => tp.Task)
            .Where(tp => tp.UserId == userId && tp.CourseId == courseId && tp.Task.TasksBlockId == blockId)
            .AsNoTracking()
            .ToListAsync();

    public async Task StoreUserTaskPointsForConcreteUserAndBlock(
        List<UserTaskPoints> userTaskPointsList, Guid userId, int courseId, int blockId)
    {
        var oldUserTaskPoints =
            (await GetUserTaskPointsByUserAndBlock(userId, courseId, blockId))
            .Select(tp => tp.TaskId)
            .ToImmutableHashSet();


        var toAdd = new List<UserTaskPoints>();
        var toUpdate = new List<UserTaskPoints>();
        foreach (var tp in userTaskPointsList)
        {
            tp.UserId = userId;
            tp.CourseId = courseId;
            if (oldUserTaskPoints.Contains(tp.TaskId))
                toUpdate.Add(tp);
            else
                toAdd.Add(tp);
        }

        await dbContext.UserTaskPoints.AddRangeAsync(toAdd);
        dbContext.UserTaskPoints.UpdateRange(toUpdate);
        await dbContext.SaveChangesAsync();
    }
}
