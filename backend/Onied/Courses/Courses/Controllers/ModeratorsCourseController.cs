using Courses.Commands;
using Courses.Dtos.Moderator.Request;
using Courses.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/courses/{id:int}/moderators")]
public class ModeratorsCourseController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetStudents(int id, [FromQuery] Guid userId)
    {
        return await sender.Send(new GetStudentsQuery(id, userId));
    }

    [HttpPatch]
    [Route("delete")]
    public async Task<IResult> DeleteModerator(
        int id,
        [FromBody] EditModeratorRequest request,
        [FromQuery] Guid userId
    )
    {
        return await sender.Send(new DeleteModeratorCommand(id, request.StudentId, userId));
    }

    [HttpPatch]
    [Route("add")]
    public async Task<IResult> AddModerator(
        int id,
        [FromBody] EditModeratorRequest request,
        [FromQuery] Guid userId
    )
    {
        return await sender.Send(new AddModeratorCommand(id, request.StudentId, userId));
    }
}
