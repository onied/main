using System.Net;
using System.Text.Json;
using Courses.Dtos;
using Courses.Enums;
using Courses.Extensions;
using Courses.Services.Abstractions;
using Courses.Services.Producers.CourseUpdatedProducer;

namespace Courses.Services;

public class SubscriptionManagementService(
    IHttpClientFactory httpClientFactory,
    IUserRepository userRepository,
    ICourseRepository courseRepository,
    ICourseUpdatedProducer courseUpdatedProducer
) : ISubscriptionManagementService
{
    public async Task<bool> VerifyGivingCertificatesAsync(Guid userId)
    {
        var subscription = await GetSubscriptionAsync(userId);
        return subscription?.CertificatesEnabled ?? false;
    }

    public async Task<bool> VerifyCreatingCoursesAsync(Guid userId)
    {
        var subscription = await GetSubscriptionAsync(userId);
        return subscription?.CourseCreatingEnabled ?? false;
    }

    public async Task SetAuthorCoursesCertificatesEnabled(Guid userId, bool status)
    {
        var user = await userRepository.GetUserWithTeachingCoursesAsync(userId);
        if (user is null) throw new ArgumentException(null, nameof(userId));

        foreach (var course in user.TeachingCourses)
        {
            course.HasCertificates = status;
            await courseRepository.UpdateCourseAsync(course);
            await courseUpdatedProducer.PublishAsync(course);
        }
    }

    public async Task SetAuthorCoursesHighlightingEnabled(Guid userId, bool status)
    {
        var user = await userRepository.GetUserWithTeachingCoursesAsync(userId);
        if (user is null) throw new ArgumentException(null, nameof(userId));

        foreach (var course in user.TeachingCourses)
        {
            course.IsGlowing = status;
            await courseRepository.UpdateCourseAsync(course);
        }
    }

    public async Task<SubscriptionRequestDto?> GetSubscriptionAsync(Guid userId)
    {
        using var client = SubscriptionsServerApiClient();
        var response = await client.GetAsync($"?userId={userId}");

        if (response.StatusCode is not HttpStatusCode.OK) return null;
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        return await JsonSerializer
            .DeserializeAsync<SubscriptionRequestDto>(
                await response.Content.ReadAsStreamAsync(), options);
    }

    private HttpClient SubscriptionsServerApiClient()
        => httpClientFactory.CreateClient(
            ServerApiConfig.SubscriptionsServer.GetStringValue()!);
}
