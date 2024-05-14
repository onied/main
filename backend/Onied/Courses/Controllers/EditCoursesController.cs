using Courses.Dtos.Course.Response;
using Courses.Dtos.EditCourse.Request;
using Courses.Filters;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/courses/{id:int}/edit")]
public class EditCoursesController(ICourseManagementService courseManagementService)
{
    [HttpPut]
    [AuthorValidationFilter]
    public async Task<IResult> EditCourse(int id,
        [FromQuery] string? userId, [FromQuery] string? role,
        [FromBody] EditCourseRequest editCourseRequest)
    {
        return await courseManagementService.EditCourse(id, editCourseRequest, userId!);
    }

    [HttpPut]
    [Route("hierarchy")]
    [AuthorValidationFilter]
    public async Task<IResult> EditHierarchy(
        int id,
        [FromQuery] string? userId, [FromQuery] string? role,
        [FromBody] CourseResponse courseResponse)
    {
        return await courseManagementService.EditHierarchy(id, courseResponse);
    }

    [HttpPost]
    [Route("add-module")]
    [AuthorValidationFilter]
    public async Task<IResult> AddModule(
        int id,
        [FromQuery] string? userId,
        [FromQuery] string? role)
    {
        return await courseManagementService.AddModule(id);
    }

    [HttpDelete]
    [Route("delete-module")]
    [AuthorValidationFilter]
    public async Task<IResult> DeleteModule(
        int id,
        [FromQuery] int moduleId,
        [FromQuery] string? userId, [FromQuery] string? role)
    {
        return await courseManagementService.DeleteModule(id, moduleId);
    }

    [HttpPut]
    [Route("rename-module")]
    [AuthorValidationFilter]
    public async Task<IResult> RenameModule(
        int id,
        [FromQuery] string? userId, [FromQuery] string? role,
        [FromBody] RenameModuleRequest renameModuleRequest)
    {
        return await courseManagementService.RenameModule(id, renameModuleRequest);
    }

    [HttpPost]
    [Route("add-block/{moduleId:int}")]
    [AuthorValidationFilter]
    public async Task<IResult> AddBlock(
        int id,
        int moduleId,
        [FromQuery] string? userId, [FromQuery] string? role,
        [FromBody] AddBlockRequest addBlockRequest)
    {
        return await courseManagementService.AddBlock(
            id, moduleId, addBlockRequest.BlockType);
    }

    [HttpDelete]
    [Route("delete-block")]
    [AuthorValidationFilter]
    public async Task<IResult> DeleteBlock(
        int id,
        [FromQuery] int blockId,
        [FromQuery] string? userId,
        [FromQuery] string? role)
    {
        return await courseManagementService.DeleteBlock(id, blockId);
    }

    [HttpPut]
    [Route("rename-block")]
    [AuthorValidationFilter]
    public async Task<IResult> RenameBlock(
        int id,
        [FromQuery] string? userId, [FromQuery] string? role,
        [FromBody] RenameBlockRequest renameBlockRequest)
    {
        return await courseManagementService.RenameBlock(id, renameBlockRequest);
    }

    [HttpPut]
    [Route("video/{blockId:int}")]
    [AuthorValidationFilter]
    public async Task<IResult> EditVideoBlock(
        int id,
        int blockId,
        [FromQuery] string? userId, [FromQuery] string? role,
        [FromBody] VideoBlockResponse videoBlockResponse)
    {
        return await courseManagementService.EditVideoBlock(id, blockId, videoBlockResponse);
    }

    [HttpPut]
    [Route("summary/{blockId:int}")]
    [AuthorValidationFilter]
    public async Task<IResult> EditSummaryBlock(
        int id,
        int blockId,
        [FromQuery] string? userId, [FromQuery] string? role,
        [FromBody] SummaryBlockResponse summaryBlockResponse)
    {
        return await courseManagementService.EditSummaryBlock(id, blockId, summaryBlockResponse);
    }

    [HttpPut]
    [Route("tasks/{blockId:int}")]
    [AuthorValidationFilter]
    public async Task<IResult> EditTasksBlock(
        int id,
        int blockId,
        [FromQuery] string? userId,
        [FromQuery] string? role,
        [FromBody] EditTasksBlockRequest tasksBlockRequest)
    {
        return await courseManagementService.EditTasksBlock(id, blockId, tasksBlockRequest);
    }

    [HttpGet]
    [Route("check-edit-course")]
    public async Task<IResult> CheckEditCourse(
        int id,
        [FromQuery] string? userId,
        [FromQuery] string? role)
    {
        return await courseManagementService.CheckCourseAuthorAsync(id, userId, role);
    }
}
