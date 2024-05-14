using Courses.Dtos.ManualReview.Request;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]/{userId:guid}")]
public class TeachingController(
    IManualReviewService manualReviewService,
    ITeachingService teachingService) : ControllerBase
{
    [HttpGet]
    [Route("authored")]
    public async Task<IResult> GetAuthoredCourses(Guid userId)
    {
        return await teachingService.GetAuthoredCourses(userId);
    }

    [HttpGet]
    [Route("moderated")]
    public async Task<IResult> GetModeratedCourses(Guid userId)
    {
        return await teachingService.GetModeratedCourses(userId);
    }

    [HttpGet]
    [Route("tasks-to-check-list")]
    public async Task<IResult> GetTasksToCheckList(Guid userId)
    {
        return await manualReviewService.GetTasksToCheckForTeacher(userId);
    }

    [HttpGet]
    [Route("check/{userAnswerId:guid}")]
    public async Task<IResult> GetTaskToCheck(
        Guid userId,
        Guid userAnswerId)
    {
        return await manualReviewService.GetManualReviewTaskUserAnswer(userId, userAnswerId);
    }

    [HttpPut]
    [Route("check/{userAnswerId:guid}")]
    public async Task<IResult> CheckTask(
        Guid userId,
        Guid userAnswerId, [FromBody] ReviewTaskRequest reviewTaskRequest)
    {
        return await manualReviewService.ReviewUserAnswer(userId, userAnswerId, reviewTaskRequest);
    }
}
