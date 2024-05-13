using Courses.Dtos;
using Courses.Filters;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/courses/{id:int}/edit")]
public class EditCoursesController(
    ILogger<CoursesController> logger,
    ICourseManagementService courseManagementService)
{
    [HttpPut]
    [AuthorValidationFilter]
    public async Task<IResult> EditCourse(int id,
        [FromQuery] string? userId,
        [FromBody] EditCourseDto editCourseDto)
    {
        return await courseManagementService.EditCourse(id, userId, editCourseDto);
    }

    [HttpPut]
    [Route("hierarchy")]
    [AuthorValidationFilter]
    public async Task<IResult> EditHierarchy(
        int id,
        [FromQuery] string? userId,
        [FromBody] CourseDto courseDto)
    {
        return await courseManagementService.EditHierarchy(id, userId, courseDto);
    }

    [HttpPost]
    [Route("add-module")]
    [AuthorValidationFilter]
    public async Task<IResult> AddModule(
        int id,
        [FromQuery] string? userId)
    {
        return await courseManagementService.AddModule(id, userId);
    }

    [HttpDelete]
    [Route("delete-module")]
    [AuthorValidationFilter]
    public async Task<IResult> DeleteModule(
        int id,
        [FromQuery] int moduleId,
        [FromQuery] string? userId)
    {
        return await courseManagementService.DeleteModule(id, moduleId, userId);
    }

    [HttpPut]
    [Route("rename-module")]
    [AuthorValidationFilter]
    public async Task<IResult> RenameModule(
        int id,
        [FromQuery] int moduleId,
        [FromQuery] string title,
        [FromQuery] string? userId)
    {
        return await courseManagementService.RenameModule(id, moduleId, title, userId);
    }

    [HttpPost]
    [Route("add-block/{moduleId:int}")]
    [AuthorValidationFilter]
    public async Task<IResult> AddBlock(
        int id,
        int moduleId,
        [FromQuery] int blockType,
        [FromQuery] string? userId)
    {
        return await courseManagementService.AddBlock(id, moduleId, blockType, userId);
    }

    [HttpDelete]
    [Route("delete-block")]
    [AuthorValidationFilter]
    public async Task<IResult> DeleteBlock(
        int id,
        [FromQuery] int blockId,
        [FromQuery] string? userId)
    {
        return await courseManagementService.DeleteBlock(id, blockId, userId);
    }

    [HttpPut]
    [Route("rename-block")]
    [AuthorValidationFilter]
    public async Task<IResult> RenameBlock(
        int id,
        [FromQuery] string? userId,
        [FromQuery] int blockId,
        [FromQuery] string title)
    {
        return await courseManagementService.RenameBlock(id, blockId, title, userId);
    }

    [HttpPut]
    [Route("video/{blockId:int}")]
    [AuthorValidationFilter]
    public async Task<IResult> EditVideoBlock(
        int id,
        int blockId,
        [FromQuery] string? userId,
        [FromBody] VideoBlockDto videoBlockDto)
    {
        return await courseManagementService.EditVideoBlock(id, blockId, userId, videoBlockDto);
    }

    [HttpPut]
    [Route("summary/{blockId:int}")]
    [AuthorValidationFilter]
    public async Task<IResult> EditSummaryBlock(
        int id,
        int blockId,
        [FromQuery] string? userId,
        [FromBody] SummaryBlockDto summaryBlockDto)
    {
        return await courseManagementService.EditSummaryBlock(id, blockId, userId, summaryBlockDto);
    }

    [HttpPut]
    [Route("tasks/{blockId:int}")]
    [AuthorValidationFilter]
    public async Task<IResult> EditTasksBlock(
        int id,
        int blockId,
        [FromQuery] string? userId,
        [FromBody] EditTasksBlockDto tasksBlockDto)
    {
        return await courseManagementService.EditTasksBlock(id, blockId, userId, tasksBlockDto);
    }

    [HttpGet]
    [Route("check-edit-course")]
    public async Task<IResult> CheckEditCourse(
        int id,
        [FromQuery] string? userId)
    {
        return await courseManagementService.CheckCourseAuthorAsync(id, userId);
    }
}
