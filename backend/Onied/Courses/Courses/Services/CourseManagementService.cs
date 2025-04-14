using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Courses.Data.Models;
using Courses.Dtos.Course.Response;
using Courses.Dtos.EditCourse.Request;
using Courses.Dtos.Moderator.Response;
using Courses.Services.Abstractions;
using Courses.Services.Producers.CourseUpdatedProducer;
using PurchasesGrpc;
using Shared;
using VerifyTokenRequest = Courses.Dtos.Course.Request.VerifyTokenRequest;

namespace Courses.Services;

public class CourseManagementService(
    ICourseRepository courseRepository,
    IUserCourseInfoRepository userCourseInfoRepository,
    IBlockRepository blockRepository,
    IUpdateTasksBlockService updateTasksBlockService,
    IModuleRepository moduleRepository,
    ICategoryRepository categoryRepository,
    ICourseUpdatedProducer courseUpdatedProducer,
    ISubscriptionManagementService subscriptionManagementService,
    IMapper mapper,
    PurchasesService.PurchasesServiceClient grpcPurchasesClient) : ICourseManagementService
{
    public async Task<IResult> CheckCourseAuthorAsync(int courseId,
        string? userId, string? role)
    {
        var course = await courseRepository.GetCourseAsync(courseId);
        if (course == null)
            return Results.NotFound();
        if (role != Roles.Admin && (userId == null || course.Author?.Id.ToString() != userId))
            return TypedResults.Forbid();

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };

        var jsonCourse = JsonSerializer.Serialize(course, options);

        return Results.Ok(jsonCourse);
    }

    public async Task<bool> AllowVisitCourse(Guid userId, int courseId, string? role = null)
    {
        if (role == Roles.Admin)
            return true;
        var userCourseInfo = await userCourseInfoRepository.GetUserCourseInfoAsync(userId, courseId, true);
        if (userCourseInfo is null) return false;
        if (userCourseInfo.Course.PriceRubles == 0) return true;

        var response =
            await grpcPurchasesClient.VerifyAsync(new PurchasesGrpc.VerifyTokenRequest
            { Token = userCourseInfo.Token });

        return response.VerificationOutcome is VerificationOutcome.Ok;
    }

    public async Task<IResult> GetStudents(int courseId, Guid authorId)
    {
        var course = await courseRepository.GetCourseWithUsersAndModeratorsAsync(courseId);
        if (course == null)
            return Results.NotFound();
        if (course.Author?.Id != authorId)
            return Results.Forbid();

        var courseDto = mapper.Map<CourseStudentsResponse>(course);
        courseDto.Students.ForEach(s => s.IsModerator = course.Moderators.Any(x => x.Id == s.StudentId));

        return Results.Ok(courseDto);
    }

    public async Task<IResult> DeleteModerator(int courseId, Guid studentId, Guid authorId)
    {
        var course = await courseRepository.GetCourseWithUsersAndModeratorsAsync(courseId);
        if (course == null)
            return Results.NotFound<string>("Курс не найден");
        if (course.Author?.Id != authorId)
            return Results.Forbid();
        if (course.Moderators.All(m => m.Id != studentId))
            return Results.NotFound<string>("Удаляемый модератор не найден");

        await courseRepository.DeleteModeratorAsync(courseId, studentId);
        return Results.Ok();
    }

    public async Task<IResult> AddModerator(int courseId, Guid studentId, Guid authorId)
    {
        var course = await courseRepository.GetCourseWithUsersAndModeratorsAsync(courseId);
        if (course == null)
            return Results.NotFound<string>("Курс не найден");

        if (course.Author?.Id != authorId)
            return Results.Forbid();

        if (course.Users.All(m => m.Id != studentId))
            return Results.NotFound<string>("Добавляемый в модераторы ученик не найден");

        await courseRepository.AddModeratorAsync(courseId, studentId);
        return Results.Ok();
    }

    public async Task<IResult> EditTasksBlock(
        int id,
        int blockId,
        EditTasksBlockRequest tasksBlockRequest)
    {
        var block = await blockRepository.GetTasksBlock(blockId);
        if (block == null || block.Module.CourseId != id)
            return Results.NotFound();

        var updatedBlock = await updateTasksBlockService.UpdateTasksBlock(tasksBlockRequest);
        var updatedBlockDto = mapper.Map<EditTasksBlockRequest>(updatedBlock);

        return Results.Ok(updatedBlockDto);
    }

    public async Task<IResult> EditSummaryBlock(
        int id,
        int blockId,
        SummaryBlockResponse summaryBlockResponse)
    {
        var block = await blockRepository.GetSummaryBlock(blockId);
        if (block == null || block.Module.CourseId != id)
            return Results.NotFound();

        mapper.Map(summaryBlockResponse, block);
        await blockRepository.UpdateSummaryBlock(block);
        return Results.Ok();
    }

    public async Task<IResult> EditVideoBlock(
        int id,
        int blockId,
        VideoBlockResponse videoBlockResponse)
    {
        var block = await blockRepository.GetVideoBlock(blockId);
        if (block == null || block.Module.CourseId != id)
            return Results.NotFound();

        mapper.Map(videoBlockResponse, block);
        await blockRepository.UpdateVideoBlock(block);
        return Results.Ok();
    }

    public async Task<IResult> RenameBlock(
        int id,
        RenameBlockRequest renameBlockRequest)
    {
        if (!await blockRepository.RenameBlockAsync(
                renameBlockRequest.BlockId, renameBlockRequest.Title))
            return Results.NotFound();

        return Results.Ok();
    }

    public async Task<IResult> DeleteBlock(
        int id,
        int blockId)
    {
        if (!await blockRepository.DeleteBlockAsync(blockId))
            return Results.NotFound();

        return Results.Ok();
    }

    public async Task<IResult> AddBlock(
        int id,
        int moduleId,
        int blockType)
    {
        var module = await moduleRepository.GetModuleAsync(moduleId);
        if (module == null)
            return Results.NotFound();

        var addedBlockId = await blockRepository.AddBlockReturnIdAsync(new Block
        {
            ModuleId = moduleId,
            Title = "Новый блок",
            BlockType = (BlockType)blockType
        });

        return Results.Ok(addedBlockId);
    }

    public async Task<IResult> RenameModule(
        int id,
        RenameModuleRequest renameModuleRequest)
    {
        if (!await moduleRepository.RenameModuleAsync(
                renameModuleRequest.ModuleId, renameModuleRequest.Title))
            return Results.NotFound();

        return Results.Ok();
    }

    public async Task<IResult> DeleteModule(
        int id,
        int moduleId)
    {
        if (!await moduleRepository.DeleteModuleAsync(moduleId))
            return Results.NotFound();

        return Results.Ok();
    }

    public async Task<IResult> AddModule(int id)
    {
        var addedModuleId = await moduleRepository.AddModuleReturnIdAsync(new Module
        {
            CourseId = id,
            Title = "Новый модуль"
        });

        return Results.Ok(addedModuleId);
    }

    public async Task<IResult> EditHierarchy(
        int id,
        CourseResponse courseResponse)
    {
        var course = await courseRepository.GetCourseAsync(id);
        if (course == null)
            return Results.NotFound();

        mapper.Map(courseResponse, course);
        await courseRepository.UpdateCourseAsync(course);

        return Results.Ok();
    }

    public async Task<IResult> EditCourse(int id,
        EditCourseRequest editCourseRequest, string userId)
    {
        var course = await courseRepository.GetCourseAsync(id);
        if (course == null)
            return Results.NotFound();

        var category = await categoryRepository.GetCategoryById(editCourseRequest.CategoryId);
        if (category == null)
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                [nameof(editCourseRequest.CategoryId)] = ["This category does not exist."]
            });

        if (editCourseRequest.HasCertificates &&
            !await subscriptionManagementService
                .VerifyGivingCertificatesAsync(Guid.Parse(userId)))
            return TypedResults.Forbid();

        mapper.Map(editCourseRequest, course);
        course.Category = category;
        course.CategoryId = category.Id;
        await courseRepository.UpdateCourseAsync(course);
        await courseUpdatedProducer.PublishAsync(course);
        return Results.Ok(mapper.Map<PreviewResponse>(course));
    }
}
