using AutoMapper;
using Courses.Dtos;
using Courses.Dtos.Course.Response;
using Courses.Dtos.EditCourse.Request;
using Courses.Models;
using Courses.Services.Abstractions;
using Courses.Services.Producers.CourseCreatedProducer;

namespace Courses.Services;

public class CourseService(
    ICourseRepository courseRepository,
    IUserCourseInfoRepository userCourseInfoRepository,
    ICourseManagementService courseManagementService,
    IBlockCompletedInfoRepository blockCompletedInfoRepository,
    IBlockRepository blockRepository,
    IUserRepository userRepository,
    ICourseCreatedProducer courseCreatedProducer,
    ICategoryRepository categoryRepository,
    ISubscriptionManagementService subscriptionManagementService,
    IMapper mapper
    ) : ICourseService
{
    public async Task<IResult> GetCoursePreview(int id, Guid? userId)
    {
        var course = await courseRepository.GetCourseAsync(id);
        if (course == null)
            return Results.NotFound();

        var userCourseInfo = userId is null
            ? null
            : await userCourseInfoRepository.GetUserCourseInfoAsync(userId.Value, id);

        var preview = mapper.Map<PreviewResponse>(course);
        preview.IsOwned = userCourseInfo is not null;
        return Results.Ok(preview);
    }

    public async Task<IResult> EnterFreeCourse(int id, Guid userId)
    {
        var course = await courseRepository.GetCourseAsync(id);
        if (course == null || course.PriceRubles > 0) return Results.NotFound();

        var maybeAlreadyEntered = await userCourseInfoRepository
            .GetUserCourseInfoAsync(userId, course.Id);
        if (maybeAlreadyEntered is not null) return Results.Forbid();
        var userCourseInfo = new UserCourseInfo()
        {
            UserId = userId,
            CourseId = course.Id
        };
        await userCourseInfoRepository.AddUserCourseInfoAsync(userCourseInfo);

        return Results.Ok();
    }

    public async Task<IResult> GetCourseHierarchy(int id, Guid userId, string? role)
    {
        var course = await courseRepository.GetCourseWithBlocksAsync(id);
        if (course == null) return Results.NotFound();

        if (!await courseManagementService.AllowVisitCourse(userId, id, role)) return Results.Forbid();

        var dto = mapper.Map<CourseResponse>(course);

        var completed =
            await blockCompletedInfoRepository
                .GetAllCompletedCourseBlocksByUser(userId, id);
        var blocksLink = dto.Modules.SelectMany(m => m.Blocks).ToList();
        foreach (var cm in completed)
            blocksLink.Single(b => b.Id == cm.BlockId).Completed = true;

        return Results.Ok(dto);
    }

    public async Task<IResult> GetSummaryBlock(int id, int blockId, Guid userId, string? role)
    {
        if (!await courseManagementService.AllowVisitCourse(userId, id, role)) return Results.Forbid();

        var summary = await blockRepository.GetSummaryBlock(blockId);
        if (summary == null || summary.Module.CourseId != id)
            return Results.NotFound();

        var dto = mapper.Map<SummaryBlockResponse>(summary);
        if (await blockCompletedInfoRepository.GetCompletedCourseBlockAsync(userId, blockId) is null)
            await blockCompletedInfoRepository.AddCompletedCourseBlockAsync(userId, blockId);
        dto.IsCompleted = true;
        return Results.Ok(dto);
    }

    public async Task<IResult> GetVideoBlock(int id, int blockId, Guid userId, string? role)
    {
        if (!await courseManagementService.AllowVisitCourse(userId, id, role)) return Results.Forbid();

        var block = await blockRepository.GetVideoBlock(blockId);
        if (block == null || block.Module.CourseId != id)
            return Results.NotFound();

        var dto = mapper.Map<VideoBlockResponse>(block);
        if (await blockCompletedInfoRepository.GetCompletedCourseBlockAsync(userId, blockId) is null)
            await blockCompletedInfoRepository.AddCompletedCourseBlockAsync(userId, blockId);
        dto.IsCompleted = true;
        return Results.Ok(dto);
    }

    public async Task<IResult> GetEditTaskBlock(int id, int blockId, Guid userId, string? role)
    {
        if (!await courseManagementService.AllowVisitCourse(userId, id, role)) return Results.Forbid();

        var block = await blockRepository.GetTasksBlock(blockId, true, true);
        if (block == null || block.Module.CourseId != id)
            return Results.NotFound();
        return Results.Ok(mapper.Map<EditTasksBlockRequest>(block));
    }

    public async Task<IResult> GetTaskBlock(int id, int blockId, Guid userId, string? role)
    {
        if (!await courseManagementService.AllowVisitCourse(userId, id, role)) return Results.Forbid();

        var block = await blockRepository.GetTasksBlock(blockId, true);
        if (block == null || block.Module.CourseId != id)
            return Results.NotFound();

        var dto = mapper.Map<TasksBlockResponse>(block);
        if (await blockCompletedInfoRepository.GetCompletedCourseBlockAsync(userId, blockId) is not null)
            dto.IsCompleted = true;
        return Results.Ok(dto);
    }

    public async Task<IResult> CreateCourse(string? userId)
    {
        if (userId == null || !Guid.TryParse(userId, out var authorId))
            return Results.Unauthorized();
        var user = await userRepository.GetUserAsync(authorId);
        if (user == null)
            return Results.Unauthorized();
        if (!await subscriptionManagementService
                .VerifyCreatingCoursesAsync(Guid.Parse(userId)))
            return Results.Forbid();

        var newCourse = await courseRepository.AddCourseAsync(new Course
        {
            AuthorId = user.Id,
            Title = "Без названия",
            Description = "Без описания",
            PictureHref = "https://upload.wikimedia.org/wikipedia/commons/3/3f/Placeholder_view_vector.svg",
            CategoryId = (await categoryRepository.GetAllCategoriesAsync())[0].Id,
            CreatedDate = DateTime.UtcNow
        });
        await courseCreatedProducer.PublishAsync(newCourse);
        return Results.Ok(new CreateCourseResponse
        {
            Id = newCourse.Id
        });
    }
}
