using AutoMapper;
using Courses.Data;
using Courses.Data.Models;
using GraphqlService.Dtos.Block.Response;
using Microsoft.EntityFrameworkCore;

namespace GraphqlService.Quiries;

public class CourseQuery(
    [Service] IHttpContextAccessor contextAccessor,
    [Service] IMapper mapper)
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Course> GetCourses(AppDbContext dbContext)
        => dbContext.Courses;

    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Course> GetOwnedCourses(AppDbContext dbContext)
    {
        var userId = contextAccessor.HttpContext!.Request.Headers["X-User-Id"].FirstOrDefault();
        if (userId is null)
        {
            throw new GraphQLException("Unauthorized access");
        }
        return dbContext.Courses
            .Where(x => x.Users.Select(y => y.Id.ToString()).Contains(userId))
            .AsQueryable();
    }

    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Course> GetPopularCourses(AppDbContext dbContext)
    {
        return dbContext.Courses
            .OrderBy(x => x.Users.Count)
            .AsQueryable();
    }

    public async Task<Course> GetCourseById(int id, AppDbContext dbContext)
    {
        var userId = contextAccessor.HttpContext!.Request.Headers["X-User-Id"].FirstOrDefault();
        if (userId is null)
        {
            throw new GraphQLException("Unauthorized access");
        }
        var course = await dbContext.Courses
            .AsNoTracking()
            .Include(course => course.Modules)
                .ThenInclude(module => module.Blocks)
            .Include(course => course.Users)
            .Include(course => course.Author)
            .Include(course => course.Category)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (course is null)
        {
            throw new GraphQLException("Course not found");
        }
        var student = course.Users.FirstOrDefault(x => x.Id.ToString() == userId);
        var moderator = course.Moderators.FirstOrDefault(x => x.Id.ToString() == userId);
        var isAuthor = course.Author?.Id.ToString() == userId;
        if (student is null && moderator is null && !isAuthor)
        {
            throw new GraphQLException("Forbidden access");
        }

        return course;
    }

    public async Task<SummaryBlockResponse> GetSummaryBlockById(int id, AppDbContext dbContext)
    {
        var userId = contextAccessor.HttpContext!.Request.Headers["X-User-Id"].FirstOrDefault();
        if (userId is null)
        {
            throw new GraphQLException("Unauthorized access");
        }
        var block = await dbContext.SummaryBlocks
            .AsNoTracking()
            .Include(x => x.Module.Course)
                .ThenInclude(course => course.Author)
            .Include(x => x.Module.Course.Users)
            .Include(x => x.Module.Course.Moderators)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (block is null)
        {
            throw new GraphQLException("Block not found");
        }
        var course = block.Module.Course;
        var student = course.Users.FirstOrDefault(x => x.Id.ToString() == userId);
        var moderator = course.Moderators.FirstOrDefault(x => x.Id.ToString() == userId);
        var isAuthor = course.Author?.Id.ToString() == userId;
        if (student is null && moderator is null && !isAuthor)
        {
            throw new GraphQLException("Forbidden access");
        }

        return mapper.Map<SummaryBlockResponse>(block);
    }

    public async Task<VideoBlockResponse> GetVideoBlockById(int id, AppDbContext dbContext)
    {
        var userId = contextAccessor.HttpContext!.Request.Headers["X-User-Id"].FirstOrDefault();
        if (userId is null)
        {
            throw new GraphQLException("Unauthorized access");
        }
        var block = await dbContext.VideoBlocks
            .AsNoTracking()
            .Include(x => x.Module.Course)
                .ThenInclude(course => course.Author)
            .Include(x => x.Module.Course.Users)
            .Include(x => x.Module.Course.Moderators)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (block is null)
        {
            throw new GraphQLException("Block not found");
        }
        var course = block.Module.Course;
        var student = course.Users.FirstOrDefault(x => x.Id.ToString() == userId);
        var moderator = course.Moderators.FirstOrDefault(x => x.Id.ToString() == userId);
        var isAuthor = course.Author?.Id.ToString() == userId;
        if (student is null && moderator is null && !isAuthor)
        {
            throw new GraphQLException("Forbidden access");
        }

        return mapper.Map<VideoBlockResponse>(block);
    }

    public async Task<TasksBlockResponse> GetTasksBlockById(int id, AppDbContext dbContext)
    {
        var userId = contextAccessor.HttpContext!.Request.Headers["X-User-Id"].FirstOrDefault();
        if (userId is null)
        {
            throw new GraphQLException("Unauthorized access");
        }
        var query = dbContext.TasksBlocks
            .Include(block => block.Module.Course)
                .ThenInclude(course => course.Author)
            .Include(x => x.Module.Course.Users)
            .Include(x => x.Module.Course.Moderators)
            .Include(block => block.Tasks)
            .AsQueryable();
        query = query.Include(block => block.Tasks)
            .ThenInclude(task => ((VariantsTask)task).Variants);
        query = query.Include(block => block.Tasks)
            .ThenInclude(task => ((InputTask)task).Answers);
        var block = await query.FirstOrDefaultAsync(x => x.Id == id);
        if (block is null)
        {
            throw new GraphQLException("Block not found");
        }

        var course = block.Module.Course;
        var student = course.Users.FirstOrDefault(x => x.Id.ToString() == userId);
        var moderator = course.Moderators.FirstOrDefault(x => x.Id.ToString() == userId);
        var isAuthor = course.Author?.Id.ToString() == userId;
        if (student is null && moderator is null && !isAuthor)
        {
            throw new GraphQLException("Forbidden access");
        }

        var dto = mapper.Map<TasksBlockResponse>(block);
        var userTaskPointsDict = dbContext.UserTaskPoints
            .AsNoTracking()
            .Where(x => block.Tasks.Select(y => y.Id).Contains(x.TaskId) &&
                        x.UserId == Guid.Parse(userId))
            .ToDictionary(x => x.TaskId, x => x.Points);
        foreach (var task in dto.Tasks)
        {
            task.Points = userTaskPointsDict.GetValueOrDefault(task.Id, 0);
        }

        return dto;
    }
}
