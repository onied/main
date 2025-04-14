using Courses.Services.Abstractions;
using MassTransit;
using MassTransit.Data.Messages;

namespace Courses.Services.Consumers;

public class SubscriptionChangedConsumer(
    ILogger<SubscriptionChangedConsumer> logger,
    ICourseUnitOfWork unitOfWork,
    ISubscriptionManagementService subscriptionManagementService
    ) : IConsumer<SubscriptionChanged>
{
    public async Task Consume(ConsumeContext<SubscriptionChanged> context)
    {
        var message = context.Message;
        logger.LogInformation("Trying to process subscription change " +
                              "(id={userId}) in database", message.UserId);
        try
        {
            await unitOfWork.BeginTransactionAsync();
            await subscriptionManagementService
                .SetAuthorCoursesCertificatesEnabled(
                    message.UserId, message.CertificatesEnabled);
            await subscriptionManagementService
                .SetAuthorCoursesHighlightingEnabled(
                    message.UserId, message.CoursesHighlightingEnabled);
            await unitOfWork.CommitTransactionAsync();
            logger.LogInformation("Processed subscription change " +
                                  "(id={userId}) in database successfully", message.UserId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed subscription change");
            await unitOfWork.RollbackTransactionAsync();
            await context.Publish(new SubscriptionChangeFailed(
                context.Message.OldSubscriptionId,
                context.Message.PurchaseId,
                context.Message.UserId,
                ex.Message));
        }
    }
}
