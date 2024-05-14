using Courses.Dtos;
using Courses.Dtos.CheckTasks.Request;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[Route("api/v1/courses/{courseId:int}/tasks/{blockId:int}")]
public class CheckTasksController(
    ILogger<CoursesController> logger,
    ICheckTaskManagementService checkTaskManagementService
) : ControllerBase
{
    [HttpGet]
    [Route("points")]
    public async Task<IResult> GetTaskPointsStored(
        int courseId,
        int blockId,
        [FromQuery] Guid userId)
    {
        return await checkTaskManagementService.GetTaskPointsStored(courseId, blockId, userId);
    }

    [HttpPost]
    [Route("check")]
    public async Task<IResult> CheckTaskBlock(
            int courseId,
            int blockId,
            [FromQuery] Guid userId,
            [FromBody] List<UserInputRequest> inputsDto)
    {
        return await checkTaskManagementService.CheckTaskBlock(courseId, blockId, userId, inputsDto);
    }
}
