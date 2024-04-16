using System.Net;
using System.Text;
using System.Text.Json;
using Courses.Dtos;
using Courses.Models;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
namespace Courses.Services;

public class CourseManagementService(
    ICourseRepository courseRepository,
    IUserCourseInfoRepository userCourseInfoRepository,
    IHttpClientFactory httpClientFactory) : ICourseManagementService
{
    public HttpClient PurchasesServerApiClient
        => httpClientFactory.CreateClient("PurchasesServer");

    public async Task<Results<Ok<Course>, NotFound, ForbidHttpResult>> CheckCourseAuthorAsync(int courseId, string? userId)
    {
        var course = await courseRepository.GetCourseAsync(courseId);
        if (course == null)
            return TypedResults.NotFound();
        if (userId == null || course.Author?.Id.ToString() != userId)
            return TypedResults.Forbid();
        return TypedResults.Ok(course);
    }

    public async Task<bool> AllowVisitCourse(Guid userId, int courseId)
    {
        var userCourseInfo = await userCourseInfoRepository.GetUserCourseInfoAsync(userId, courseId);
        if (userCourseInfo is null) return false;

        var requestString = JsonSerializer.Serialize(new VerifyTokenRequestDto(userCourseInfo.Token));
        var response =
            await PurchasesServerApiClient.PostAsync(
                "api/v1/Purchases/verify",
                new StringContent(requestString, Encoding.UTF8, "application/json"));

        return response.StatusCode is HttpStatusCode.OK;
    }
}
