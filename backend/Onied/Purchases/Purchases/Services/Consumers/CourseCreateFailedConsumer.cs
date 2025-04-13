using MassTransit;
using MassTransit.Data.Messages;
using Purchases.Data.Abstractions;
using Purchases.Data.Models;

namespace Purchases.Services.Consumers;

public class CourseCreateFailedConsumer(
    ILogger<CourseCreateFailedConsumer> logger,
    IPurchaseUnitOfWork unitOfWork) : IConsumer<CourseCreateFailed>
{
    public async Task Consume(ConsumeContext<CourseCreateFailed> context)
    {
        logger.LogWarning("Failed to create course with id {0}: {1}", context.Message.Id, context.Message.ErrorMessage);
        await unitOfWork.BeginTransactionAsync();
        await unitOfWork.Purchases.RemoveAsyncByCourseId(context.Message.Id);
        await unitOfWork.Courses.RemoveAsync(new Course { Id = context.Message.Id });
        await unitOfWork.CommitTransactionAsync();
    }
}
