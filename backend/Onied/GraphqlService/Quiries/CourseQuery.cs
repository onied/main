using AutoMapper;
using Courses.Data;
using Courses.Data.Models;
using GraphqlService.Dtos.Course.Response;
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

    public async Task<Course> GetCourseById(
        int id,
        AppDbContext dbContext)
    {
        var userId = contextAccessor.HttpContext!.Request.Headers["X-User-Id"].FirstOrDefault();
        if (userId is null)
        {
            throw new GraphQLException("Unauthorized access");
        }
        var course = await dbContext.Courses
            .AsNoTracking()
            .Include(course => course.Modules)
            .Include(course => course.Users)
            .Include(course => course.Author)
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

    public async Task<SummaryBlockResponse> GetSummaryBlockById(
        int id,
        AppDbContext dbContext)
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

    public async Task<VideoBlockResponse> GetVideoBlockById(
        int id,
        AppDbContext dbContext)
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

    public async Task<TasksBlockResponse> GetTasksBlockById(
        int id,
        AppDbContext dbContext)
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
        return mapper.Map<TasksBlockResponse>(block);
    }
}
