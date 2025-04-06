using AutoMapper;
using Courses.Dtos;
using Courses.Services.Abstractions;
using Courses.Services.Producers.CourseUpdatedProducer;
using Google.Protobuf;
using Grpc.Core;
using PurchasesGrpc;

namespace Courses.Services;

public class SubscriptionManagementService(
    IUserRepository userRepository,
    ICourseRepository courseRepository,
    ICourseUpdatedProducer courseUpdatedProducer,
    SubscriptionService.SubscriptionServiceClient grpcPurchasesClient,
    IMapper mapper
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
        try
        {
            var subscription =
                await grpcPurchasesClient.GetActiveSubscriptionAsync(new GetActiveSubscriptionRequest
                    { UserId = ByteString.CopyFrom(userId.ToByteArray()) });

            return mapper.Map<SubscriptionRequestDto>(subscription);
        }
        catch (RpcException _)
        {
            return null;
        }
    }
}
