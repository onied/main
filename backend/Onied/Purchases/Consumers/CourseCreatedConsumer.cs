using AutoMapper;
using MassTransit;
using MassTransit.Data.Messages;
using Purchases.Abstractions;
using Purchases.Data.Abstractions;
using Purchases.Data.Enums;
using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;
using Purchases.Producers.PurchaseCreatedProducer;

namespace Purchases.Consumers;

public class CourseCreatedConsumer(
    ILogger<CourseCreatedConsumer> logger,
    IMapper mapper,
    ICourseRepository courseRepository,
    IPurchaseRepository purchaseRepository,
    IPurchaseTokenService tokenService,
    IPurchaseCreatedProducer purchaseCreatedProducer) : IConsumer<CourseCreated>
{
    public async Task Consume(ConsumeContext<CourseCreated> context)
    {
        var course = mapper.Map<Course>(context.Message);
        logger.LogInformation("Trying to create course(id={courseId}) in database", course.Id);
        await courseRepository.AddAsync(course);
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
        purchase = await purchaseRepository.AddAsync(purchase, purchaseDetails);
        purchase.Token = tokenService.GetToken(purchase);
        await purchaseRepository.UpdateAsync(purchase);
        await purchaseCreatedProducer.PublishAsync(purchase);
    }
}
