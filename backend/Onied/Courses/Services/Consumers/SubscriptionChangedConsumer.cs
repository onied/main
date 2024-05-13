using Courses.Services.Abstractions;
using MassTransit;
using MassTransit.Data.Messages;

namespace Courses.Services.Consumers;

public class SubscriptionChangedConsumer(
    ILogger<SubscriptionChangedConsumer> logger,
    ISubscriptionManagementService subscriptionManagementService
    ) : IConsumer<SubscriptionChanged>
{
    public async Task Consume(ConsumeContext<SubscriptionChanged> context)
    {
        var message = context.Message;
        logger.LogInformation("Trying to process subscription change " +
                              "(id={userId}) in database", message.UserId);

        await subscriptionManagementService
            .SetAuthorCoursesCertificatesEnabled(
                message.UserId, message.CertificatesEnabled);
        await subscriptionManagementService
            .SetAuthorCoursesHighlightingEnabled(
                message.UserId, message.CoursesHighlightingEnabled);

        logger.LogInformation("Processed subscription change " +
                              "(id={userId}) in database successfully", message.UserId);
    }
}
