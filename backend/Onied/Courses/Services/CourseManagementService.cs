using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Courses.Dtos;
using AutoMapper;
using Courses.Dtos.ModeratorDtos.Response;
using Courses.Enums;
using Courses.Extensions;
using Courses.Models;
using Courses.Services.Abstractions;
using Courses.Services.Producers.CourseUpdatedProducer;

namespace Courses.Services;

public class CourseManagementService(
    ICourseRepository courseRepository,
    IUserCourseInfoRepository userCourseInfoRepository,
    IHttpClientFactory httpClientFactory,
    IBlockRepository blockRepository,
    IUpdateTasksBlockService updateTasksBlockService,
    IModuleRepository moduleRepository,
    ICategoryRepository categoryRepository,
    ICourseUpdatedProducer courseUpdatedProducer,
    IMapper mapper) : ICourseManagementService
{
    public HttpClient PurchasesServerApiClient
        => httpClientFactory.CreateClient(ServerApiConfig.PurchasesServer.GetStringValue()!);

    public async Task<IResult> CheckCourseAuthorAsync(int courseId, string? userId)
    {
        var course = await courseRepository.GetCourseAsync(courseId);
        if (course == null)
            return Results.NotFound();

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };

        string jsonCourse = JsonSerializer.Serialize(course, options);

        return Results.Ok(jsonCourse);
    }

    public async Task<bool> AllowVisitCourse(Guid userId, int courseId)
    {
        var userCourseInfo = await userCourseInfoRepository.GetUserCourseInfoAsync(userId, courseId, true);
        if (userCourseInfo is null) return false;
        if (userCourseInfo.Course.PriceRubles == 0) return true;

        var requestString = JsonSerializer.Serialize(new VerifyTokenRequestDto(userCourseInfo.Token!));
        var response =
            await PurchasesServerApiClient.PostAsync(
                string.Empty,
                new StringContent(requestString, Encoding.UTF8, "application/json"));

        return response.StatusCode is HttpStatusCode.OK;
    }

    public async Task<IResult> GetStudents(int courseId, Guid authorId)
    {
        var course = await courseRepository.GetCourseWithUsersAndModeratorsAsync(courseId);
        if (course == null)
            return Results.NotFound();
        if (course.Author?.Id != authorId)
            return Results.Forbid();

        var courseDto = mapper.Map<CourseStudentsDto>(course);
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
        string? userId,
        EditTasksBlockDto tasksBlockDto)
    {
        var block = await blockRepository.GetTasksBlock(blockId);
        if (block == null || block.Module.CourseId != id)
            return Results.NotFound();

        var updatedBlock = await updateTasksBlockService.UpdateTasksBlock(tasksBlockDto);
        var updatedBlockDto = mapper.Map<EditTasksBlockDto>(updatedBlock);

        return Results.Ok(updatedBlockDto);
    }

    public async Task<IResult> EditSummaryBlock(
        int id,
        int blockId,
        string? userId,
        SummaryBlockDto summaryBlockDto)
    {
        var block = await blockRepository.GetSummaryBlock(blockId);
        if (block == null || block.Module.CourseId != id)
            return Results.NotFound();

        mapper.Map(summaryBlockDto, block);
        await blockRepository.UpdateSummaryBlock(block);
        return Results.Ok();
    }

    public async Task<IResult> EditVideoBlock(
        int id,
        int blockId,
        string? userId,
        VideoBlockDto videoBlockDto)
    {
        var block = await blockRepository.GetVideoBlock(blockId);
        if (block == null || block.Module.CourseId != id)
            return Results.NotFound();

        mapper.Map(videoBlockDto, block);
        await blockRepository.UpdateVideoBlock(block);
        return Results.Ok();
    }

    public async Task<IResult> RenameBlock(
        int id,
        RenameBlockDto renameBlockDto,
        string? userId)
    {

        if (!await blockRepository.RenameBlockAsync(
                renameBlockDto.BlockId, renameBlockDto.Title))
            return Results.NotFound();

        return Results.Ok();
    }

    public async Task<IResult> DeleteBlock(
        int id,
        int blockId,
        string? userId)
    {
        if (!await blockRepository.DeleteBlockAsync(blockId))
            return Results.NotFound();

        return Results.Ok();
    }

    public async Task<IResult> AddBlock(
        int id,
        int moduleId,
        int blockType,
        string? userId)
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
        string? userId,
        RenameModuleDto renameModuleDto)
    {
        if (!await moduleRepository.RenameModuleAsync(
                renameModuleDto.ModuleId, renameModuleDto.Title))
            return Results.NotFound();

        return Results.Ok();
    }

    public async Task<IResult> DeleteModule(
        int id,
        int moduleId,
        string? userId)
    {
        if (!await moduleRepository.DeleteModuleAsync(moduleId))
            return Results.NotFound();

        return Results.Ok();
    }

    public async Task<IResult> AddModule(
        int id,
        string? userId)
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
        string? userId,
        CourseDto courseDto)
    {
        var course = await courseRepository.GetCourseAsync(id);
        if (course == null)
            return Results.NotFound();

        mapper.Map(courseDto, course);
        await courseRepository.UpdateCourseAsync(course);

        return Results.Ok();
    }

    public async Task<IResult> EditCourse(int id,
        string? userId,
        EditCourseDto editCourseDto)
    {
        var course = await courseRepository.GetCourseAsync(id);
        if (course == null)
            return Results.NotFound();

        var category = await categoryRepository.GetCategoryById(editCourseDto.CategoryId);
        if (category == null)
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                [nameof(editCourseDto.CategoryId)] = ["This category does not exist."]
            });

        mapper.Map(editCourseDto, course);
        course.Category = category;
        course.CategoryId = category.Id;
        await courseRepository.UpdateCourseAsync(course);
        await courseUpdatedProducer.PublishAsync(course);
        return Results.Ok(mapper.Map<PreviewDto>(course));
    }

}
