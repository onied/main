using AutoMapper;
using Courses.Dtos;
using Courses.Models;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/courses/{id:int}/edit")]
public class EditCoursesController(
    ILogger<CoursesController> logger,
    IMapper mapper,
    ICourseRepository courseRepository,
    IBlockRepository blockRepository,
    ICourseManagementService courseManagementService,
    ICategoryRepository categoryRepository,
    IModuleRepository moduleRepository,
    IUpdateTasksBlockService updateTasksBlockService)
{
    [HttpPut]
    public async Task<Results<Ok<PreviewDto>, NotFound, ValidationProblem, ForbidHttpResult>> EditCourse(int id,
        [FromQuery] string? userId,
        [FromBody] EditCourseDto editCourseDto)
    {
        var response = await courseManagementService.CheckCourseAuthorAsync(id, userId);
        if (!response.TryPickT0(out var ok, out var remainder))
            return remainder.Match<Results<Ok<PreviewDto>, NotFound, ValidationProblem, ForbidHttpResult>>(
                notFound => notFound, forbid => forbid);

        var course = ok.Value!;
        var category = await categoryRepository.GetCategoryById(editCourseDto.CategoryId);
        if (category == null)
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                [nameof(editCourseDto.CategoryId)] = ["This category does not exist."]
            });
        mapper.Map(editCourseDto, course);
        course.Category = category;
        course.CategoryId = category.Id;
        await courseRepository.UpdateCourseAsync(course);
        return TypedResults.Ok(mapper.Map<PreviewDto>(course));
    }

    [HttpPut]
    [Route("hierarchy")]
    public async Task<Results<Ok, NotFound, ForbidHttpResult>> EditHierarchy(
        int id,
        [FromQuery] string? userId,
        [FromBody] CourseDto courseDto)
    {
        var response = await courseManagementService.CheckCourseAuthorAsync(id, userId);
        if (!response.TryPickT0(out var ok, out var remainder))
            return remainder.Match<Results<Ok, NotFound, ForbidHttpResult>>(notFound => notFound, forbid => forbid);

        var course = ok.Value!;
        mapper.Map(courseDto, course);
        await courseRepository.UpdateCourseAsync(course);

        return TypedResults.Ok();
    }

    [HttpPost]
    [Route("add-module")]
    public async Task<Results<Ok<int>, NotFound, ForbidHttpResult>> AddModule(
        int id,
        [FromQuery] string? userId)
    {
        var response = await courseManagementService.CheckCourseAuthorAsync(id, userId);
        if (!response.TryPickT0(out _, out var remainder))
            return remainder.Match<Results<Ok<int>, NotFound, ForbidHttpResult>>(notFound => notFound,
                forbid => forbid);

        var addedModuleId = await moduleRepository.AddModuleReturnIdAsync(new Module
        {
            CourseId = id,
            Title = "Новый модуль"
        });

        return TypedResults.Ok(addedModuleId);
    }

    [HttpDelete]
    [Route("delete-module")]
    public async Task<Results<Ok, NotFound, ForbidHttpResult>> DeleteModule(
        int id,
        [FromQuery] int moduleId,
        [FromQuery] string? userId)
    {
        var response = await courseManagementService.CheckCourseAuthorAsync(id, userId);
        if (!response.TryPickT0(out _, out var remainder))
            return remainder.Match<Results<Ok, NotFound, ForbidHttpResult>>(notFound => notFound, forbid => forbid);

        if (!await moduleRepository.DeleteModuleAsync(moduleId))
            return TypedResults.NotFound();

        return TypedResults.Ok();
    }

    [HttpPut]
    [Route("rename-module")]
    public async Task<Results<Ok, NotFound, ForbidHttpResult>> RenameModule(
        int id,
        [FromQuery] int moduleId,
        [FromQuery] string title,
        [FromQuery] string? userId)
    {
        var response = await courseManagementService.CheckCourseAuthorAsync(id, userId);
        if (!response.TryPickT0(out _, out var remainder))
            return remainder.Match<Results<Ok, NotFound, ForbidHttpResult>>(notFound => notFound, forbid => forbid);

        if (!await moduleRepository.RenameModuleAsync(moduleId, title))
            return TypedResults.NotFound();

        return TypedResults.Ok();
    }

    [HttpPost]
    [Route("add-block/{moduleId:int}")]
    public async Task<Results<Ok<int>, NotFound, ForbidHttpResult>> AddBlock(
        int id,
        int moduleId,
        [FromQuery] int blockType,
        [FromQuery] string? userId)
    {
        var response = await courseManagementService.CheckCourseAuthorAsync(id, userId);
        if (!response.TryPickT0(out _, out var remainder))
            return remainder.Match<Results<Ok<int>, NotFound, ForbidHttpResult>>(notFound => notFound,
                forbid => forbid);

        var module = await moduleRepository.GetModuleAsync(moduleId);
        if (module == null)
            return TypedResults.NotFound();

        var addedBlockId = await blockRepository.AddBlockReturnIdAsync(new Block
        {
            ModuleId = moduleId,
            Title = "Новый блок",
            BlockType = (BlockType)blockType
        });

        return TypedResults.Ok(addedBlockId);
    }

    [HttpDelete]
    [Route("delete-block")]
    public async Task<Results<Ok, NotFound, ForbidHttpResult>> DeleteBlock(
        int id,
        [FromQuery] int blockId,
        [FromQuery] string? userId)
    {
        var response = await courseManagementService.CheckCourseAuthorAsync(id, userId);
        if (!response.TryPickT0(out _, out var remainder))
            return remainder.Match<Results<Ok, NotFound, ForbidHttpResult>>(notFound => notFound, forbid => forbid);

        if (!await blockRepository.DeleteBlockAsync(blockId))
            return TypedResults.NotFound();

        return TypedResults.Ok();
    }

    [HttpPut]
    [Route("rename-block")]
    public async Task<Results<Ok, NotFound, ForbidHttpResult>> RenameBlock(
        int id,
        [FromQuery] int blockId,
        [FromQuery] string title,
        [FromQuery] string? userId)
    {
        var response = await courseManagementService.CheckCourseAuthorAsync(id, userId);
        if (!response.TryPickT0(out _, out var remainder))
            return remainder.Match<Results<Ok, NotFound, ForbidHttpResult>>(notFound => notFound, forbid => forbid);

        if (!await blockRepository.RenameBlockAsync(blockId, title))
            return TypedResults.NotFound();

        return TypedResults.Ok();
    }

    [HttpPut]
    [Route("video/{blockId:int}")]
    public async Task<Results<Ok, NotFound, ForbidHttpResult>> EditVideoBlock(
        int id,
        int blockId,
        [FromQuery] string? userId,
        [FromBody] VideoBlockDto videoBlockDto)
    {
        var response = await courseManagementService.CheckCourseAuthorAsync(id, userId);
        if (!response.TryPickT0(out _, out var remainder))
            return remainder.Match<Results<Ok, NotFound, ForbidHttpResult>>(notFound => notFound, forbid => forbid);

        var block = await blockRepository.GetVideoBlock(blockId);
        if (block == null || block.Module.CourseId != id)
            return TypedResults.NotFound();

        mapper.Map(videoBlockDto, block);
        await blockRepository.UpdateVideoBlock(block);
        return TypedResults.Ok();
    }

    [HttpPut]
    [Route("summary/{blockId:int}")]
    public async Task<Results<Ok, NotFound, ForbidHttpResult>> EditSummaryBlock(
        int id,
        int blockId,
        [FromQuery] string? userId,
        [FromBody] SummaryBlockDto summaryBlockDto)
    {
        var response = await courseManagementService.CheckCourseAuthorAsync(id, userId);
        if (!response.TryPickT0(out _, out var remainder))
            return remainder.Match<Results<Ok, NotFound, ForbidHttpResult>>(notFound => notFound, forbid => forbid);

        var block = await blockRepository.GetSummaryBlock(blockId);
        if (block == null || block.Module.CourseId != id)
            return TypedResults.NotFound();

        mapper.Map(summaryBlockDto, block);
        await blockRepository.UpdateSummaryBlock(block);
        return TypedResults.Ok();
    }

    [HttpPut]
    [Route("tasks/{blockId:int}")]
    public async Task<Results<Ok<EditTasksBlockDto>, NotFound, ForbidHttpResult>> EditTasksBlock(
        int id,
        int blockId,
        [FromQuery] string? userId,
        [FromBody] EditTasksBlockDto tasksBlockDto)
    {
        var response = await courseManagementService.CheckCourseAuthorAsync(id, userId);
        if (!response.TryPickT0(out _, out var remainder))
            return remainder.Match<Results<Ok<EditTasksBlockDto>, NotFound, ForbidHttpResult>>(
                notFound => notFound,
                forbid => forbid);

        var block = await blockRepository.GetTasksBlock(blockId);
        if (block == null || block.Module.CourseId != id)
            return TypedResults.NotFound();


        var updatedBlock = await updateTasksBlockService.UpdateTasksBlock(tasksBlockDto);
        var updatedBlockDto = mapper.Map<EditTasksBlockDto>(updatedBlock);

        return TypedResults.Ok(updatedBlockDto);
    }

    [HttpGet]
    [Route("check-edit-course")]
    public async Task<Results<Ok, NotFound, ForbidHttpResult>> CheckEditCourse(
        int id,
        [FromQuery] string? userId)
    {
        var response = await courseManagementService.CheckCourseAuthorAsync(id, userId);
        return response.Match<Results<Ok, NotFound, ForbidHttpResult>>(
            _ => TypedResults.Ok(),
            notFound => notFound,
            forbid => forbid);
    }
}
