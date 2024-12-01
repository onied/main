using Courses.Commands;
using Courses.Dtos.ManualReview.Request;
using Courses.Queries;
using Courses.Services.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]/{userId:guid}")]
public class TeachingController(ISender sender, IManualReviewService manualReviewService)
    : ControllerBase
{
    [HttpGet]
    [Route("authored")]
    public async Task<IResult> GetAuthoredCourses(Guid userId)
    {
        return await sender.Send(new GetAuthoredCoursesQuery(userId));
    }

    [HttpGet]
    [Route("moderated")]
    public async Task<IResult> GetModeratedCourses(Guid userId)
    {
        return await sender.Send(new GetModeratedCoursesQuery(userId));
    }

    [HttpGet]
    [Route("tasks-to-check-list")]
    public async Task<IResult> GetTasksToCheckList(Guid userId)
    {
        return await sender.Send(new GetTasksToCheckListQuery(userId));
    }

    [HttpGet]
    [Route("check/{userAnswerId:guid}")]
    public async Task<IResult> GetTaskToCheck(Guid userId, Guid userAnswerId)
    {
        return await sender.Send(new GetTaskToCheckQuery(userId, userAnswerId));
    }

    [HttpPut]
    [Route("check/{userAnswerId:guid}")]
    public async Task<IResult> CheckTask(
        Guid userId,
        Guid userAnswerId,
        [FromBody] ReviewTaskRequest reviewTaskRequest
    )
    {
        return await sender.Send(
            new AddCheckedTaskCommand(userId, userAnswerId, reviewTaskRequest)
        );
    }
}
