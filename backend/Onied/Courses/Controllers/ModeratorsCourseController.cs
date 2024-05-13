using AutoMapper;
using Courses.Dtos.ModeratorDtos.Response;
using Courses.Services;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/courses/{id:int}/moderators")]
public class ModeratorsCourseController(
    ICourseManagementService courseManagementService
    ) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetStudents(int id, [FromQuery] Guid userId)
    {
        return await courseManagementService.GetStudents(id, userId);
    }

    [HttpPatch]
    [Route("delete")]
    public async Task<IResult> DeleteModerator(
        int id,
        [FromQuery] Guid studentId,
        [FromQuery] Guid userId
        )
    {
        return await courseManagementService.DeleteModerator(id, studentId, userId);
    }

    [HttpPatch]
    [Route("add")]
    public async Task<IResult> AddModerator(
        int id,
        [FromQuery] Guid studentId,
        [FromQuery] Guid userId
        )
    {
        return await courseManagementService.AddModerator(id, studentId, userId);
    }
}
