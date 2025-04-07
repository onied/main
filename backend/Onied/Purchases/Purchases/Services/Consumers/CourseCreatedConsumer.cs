using AutoMapper;
using MassTransit;
using MassTransit.Data.Messages;
using Purchases.Data.Abstractions;
using Purchases.Data.Enums;
using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;
using Purchases.Services.Abstractions;

namespace Purchases.Services.Consumers;

public class CourseCreatedConsumer(
    ILogger<CourseCreatedConsumer> logger,
    IMapper mapper,
    IPurchaseUnitOfWork unitOfWork,
    IPurchaseTokenService tokenService,
    IPurchaseCreatedProducer purchaseCreatedProducer) : IConsumer<CourseCreated>
{
    public async Task Consume(ConsumeContext<CourseCreated> context)
    {
        try
        {
            await unitOfWork.BeginTransactionAsync();
            var course = mapper.Map<Course>(context.Message);
            logger.LogInformation("Trying to create course(id={courseId}) in database", course.Id);
            await unitOfWork.Courses.AddAsync(course);
            logger.LogInformation("Created course(id={courseId}) in database", course.Id);

            var purchase = new Purchase()
            {
                UserId = course.AuthorId,
                Price = 0
            };
            var purchaseDetails = new CoursePurchaseDetails()
            {
                CourseId = course.Id,
                PurchaseType = PurchaseType.Course
            };
            purchase = await unitOfWork.Purchases.AddAsync(purchase, purchaseDetails);
            purchase.Token = tokenService.GetToken(purchase);
            await unitOfWork.Purchases.UpdateAsync(purchase);
            throw new Exception();
            await unitOfWork.CommitTransactionAsync();
            await purchaseCreatedProducer.PublishAsync(purchase);
        }
        catch (Exception ex)
        {
            logger.LogWarning("Failed to create course: {0}", ex.Message);
            await unitOfWork.RollbackTransactionAsync();
            await context.Publish(new CourseCreateFailed(context.Message.Id, ex.Message));
        }
    }
}
