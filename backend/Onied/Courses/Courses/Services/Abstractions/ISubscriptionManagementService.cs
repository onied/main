using Courses.Dtos;

namespace Courses.Services.Abstractions;

public interface ISubscriptionManagementService
{
    public Task<bool> VerifyGivingCertificatesAsync(Guid userId);
    public Task<bool> VerifyCreatingCoursesAsync(Guid userId);
    public Task SetAuthorCoursesCertificatesEnabled(Guid userId, bool status);
    public Task SetAuthorCoursesHighlightingEnabled(Guid userId, bool status);
    public Task<SubscriptionRequestDto?> GetSubscriptionAsync(Guid userId);
}
