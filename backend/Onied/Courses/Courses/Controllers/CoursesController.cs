using Courses.Commands;
using Courses.Filters;
using Courses.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]/{id:int}")]
public class CoursesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetCoursePreview(int id, [FromQuery] Guid? userId)
    {
        return await sender.Send(new GetCoursePreviewQuery(id, userId));
    }

    [HttpPost("like")]
    public async Task<IResult> LikeCourse(int id, [FromQuery] Guid userId)
    {
        return await sender.Send(new LikeCourseCommand(id, userId, true));
    }

    [HttpPost("unlike")]
    public async Task<IResult> UnlikeCourse(int id, [FromQuery] Guid userId)
    {
        return await sender.Send(new LikeCourseCommand(id, userId, false));
    }

    [HttpPost("enter")]
    public async Task<IResult> EnterFreeCourse(int id, [FromQuery] Guid userId)
    {
        return await sender.Send(new EnterFreeCourseCommand(id, userId));
    }

    [HttpGet]
    [Route("hierarchy")]
    [AllowVisitCourseValidationFilter]
    public async Task<IResult> GetCourseHierarchy(int id, [FromQuery] Guid userId,
        [FromQuery] string? role)
    {
        return await sender.Send(new GetCourseHierarchyQuery(id, userId, role));
    }

    [HttpGet]
    [Route("summary/{blockId:int}")]
    [AllowVisitCourseValidationFilter]
    public async Task<IResult> GetSummaryBlock(int id, int blockId, [FromQuery] Guid userId,
        [FromQuery] string? role)
    {
        return await sender.Send(new GetSummaryBlockQuery(id, blockId, userId, role));
    }

    [HttpGet]
    [Route("video/{blockId:int}")]
    [AllowVisitCourseValidationFilter]
    public async Task<IResult> GetVideoBlock(int id, int blockId, [FromQuery] Guid userId,
        [FromQuery] string? role)
    {
        return await sender.Send(new GetVideoBlockQuery(id, blockId, userId, role));
    }

    [HttpGet]
    [Route("tasks/{blockId:int}/for-edit")]
    [AllowVisitCourseValidationFilter]
    public async Task<IResult> GetEditTaskBlock(int id, int blockId, [FromQuery] Guid userId,
        [FromQuery] string? role)
    {
        return await sender.Send(new GetEditTaskBlockQuery(id, blockId, userId, role));
    }

    [HttpGet]
    [Route("tasks/{blockId:int}")]
    [AllowVisitCourseValidationFilter]
    public async Task<IResult> GetTaskBlock(int id, int blockId, [FromQuery] Guid userId,
        [FromQuery] string? role)
    {
        return await sender.Send(new GetTaskBlockQuery(id, blockId, userId, role));
    }

    [HttpPost]
    [Route("/api/v1/[controller]/create")]
    public async Task<IResult> CreateCourse(
        [FromQuery] string? userId)
    {
        return await sender.Send(new CreateCourseCommand(userId));
    }
}
