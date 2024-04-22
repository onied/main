using System.Net;
using System.Text.Json;
using Courses.Dtos;
using Courses.Enums;
using Courses.Extensions;
using Courses.Services.Abstractions;

namespace Courses.Services;

public class SubscriptionManagementService(
    IHttpClientFactory httpClientFactory
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

    private HttpClient SubscriptionsServerApiClient(Guid userId)
        => httpClientFactory.CreateClient(
            ServerApiConfig.SubscriptionsServer.GetStringValue()!
            + $"?userId={userId}");

    private async Task<SubscriptionRequestDto?> GetSubscriptionAsync(Guid userId)
    {
        using var client = SubscriptionsServerApiClient(userId);
        var response = await client.GetAsync(string.Empty);

        if (response.StatusCode is not HttpStatusCode.OK) return null;
        return await JsonSerializer
            .DeserializeAsync<SubscriptionRequestDto>(
                await response.Content.ReadAsStreamAsync());
    }
}
