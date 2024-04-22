namespace Courses.Services.Abstractions;

public interface ISubscriptionManagementService
{
    public Task<bool> VerifyGivingCertificatesAsync(Guid userId);
    public Task<bool> VerifyCreatingCoursesAsync(Guid userId);
}
