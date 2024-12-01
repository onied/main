using Courses.Commands;
using Courses.Dtos.CheckTasks.Request;
using Courses.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[Route("api/v1/courses/{courseId:int}/tasks/{blockId:int}")]
public class CheckTasksController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("points")]
    public async Task<IResult> GetTaskPointsStored(
        int courseId,
        int blockId,
        [FromQuery] Guid userId,
        [FromQuery] string? role
    )
    {
        return await sender.Send(new GetTaskPointsStoredQuery(courseId, blockId, userId, role));
    }

    [HttpPost]
    [Route("check")]
    public async Task<IResult> CheckTaskBlock(
        int courseId,
        int blockId,
        [FromQuery] Guid userId,
        [FromQuery] string? role,
        [FromBody] List<UserInputRequest> inputsDto
    )
    {
        return await sender.Send(
            new EditCheckedTaskBlockCommand(courseId, blockId, userId, role, inputsDto)
        );
    }
}
