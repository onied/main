using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Purchases.Data.Abstractions;
using Purchases.Data.Enums;
using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;
using Purchases.Dtos;
using Purchases.Dtos.Requests;
using Purchases.Dtos.Responses;
using Purchases.Services;
using Purchases.Services.Abstractions;

namespace Tests.Purchases.UnitTests.ServiceTests;

public class PurchaseMakingServiceTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IValidatePurchaseService> _mockValidatePurchaseService;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ICourseRepository> _mockCourseRepository;
    private readonly Mock<ISubscriptionRepository> _mockSubscriptionRepository;
    private readonly Mock<IPurchaseRepository> _mockPurchaseRepository;
    private readonly Mock<IPurchaseTokenService> _mockTokenService;
    private readonly Mock<IPurchaseCreatedProducer> _mockPurchaseCreatedProducer;
    private readonly Mock<ISubscriptionChangedProducer> _mockSubscriptionChangedProducer;
    private readonly PurchaseMakingService _purchaseMakingService;

    public PurchaseMakingServiceTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockValidatePurchaseService = new Mock<IValidatePurchaseService>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockCourseRepository = new Mock<ICourseRepository>();
        _mockSubscriptionRepository = new Mock<ISubscriptionRepository>();
        _mockPurchaseRepository = new Mock<IPurchaseRepository>();
        _mockTokenService = new Mock<IPurchaseTokenService>();
        _mockPurchaseCreatedProducer = new Mock<IPurchaseCreatedProducer>();
        _mockSubscriptionChangedProducer = new Mock<ISubscriptionChangedProducer>();
        _purchaseMakingService = new PurchaseMakingService(
            _mockMapper.Object,
            _mockValidatePurchaseService.Object,
            _mockUserRepository.Object,
            _mockCourseRepository.Object,
            _mockSubscriptionRepository.Object,
            _mockPurchaseRepository.Object,
            _mockTokenService.Object,
            _mockPurchaseCreatedProducer.Object,
            _mockSubscriptionChangedProducer.Object
        );
    }

    [Fact]
    public async Task GetCoursePreparedPurchase_CourseNotFound_ReturnsNotFound()
    {
        _mockCourseRepository.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync((Course)null);

        var result = await _purchaseMakingService.GetCoursePreparedPurchase(1);

        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetCoursePreparedPurchase_CourseFound_ReturnsOk()
    {
        var course = new Course { Id = 1, Title = "Test Course" };
        _mockCourseRepository.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(course);
        _mockMapper
            .Setup(m => m.Map<PreparedPurchaseResponseDto>(It.IsAny<Course>()))
            .Returns(new PreparedPurchaseResponseDto(It.IsAny<string>(), It.IsAny<decimal>()));

        var result = await _purchaseMakingService.GetCoursePreparedPurchase(1);

        Assert.IsType<Ok<PreparedPurchaseResponseDto>>(result);
    }

    [Fact]
    public async Task MakeCoursePurchase_ValidationFails_ReturnsErrorResult()
    {
        var dto = new PurchaseRequestDto(
            It.IsAny<Guid>(),
            It.IsAny<decimal>(),
            It.IsAny<CardInfoDto>(),
            It.IsAny<PurchaseType>(),
            It.IsAny<int>(),
            It.IsAny<int>()
        );
        _mockValidatePurchaseService
            .Setup(x =>
                x.ValidatePurchase(It.IsAny<PurchaseRequestDto>(), It.IsAny<PurchaseType>())
            )
            .ReturnsAsync(Results.BadRequest("Validation Failed"));

        var result = await _purchaseMakingService.MakeCoursePurchase(dto, Guid.NewGuid());

        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
    public async Task MakeCoursePurchase_ValidationPasses_ReturnsOk()
    {
        var dto = new PurchaseRequestDto(
            It.IsAny<Guid>(),
            It.IsAny<decimal>(),
            It.IsAny<CardInfoDto>(),
            It.IsAny<PurchaseType>(),
            It.IsAny<int>(),
            It.IsAny<int>()
        );
        var purchase = new Purchase();
        _mockValidatePurchaseService
            .Setup(x =>
                x.ValidatePurchase(It.IsAny<PurchaseRequestDto>(), It.IsAny<PurchaseType>())
            )
            .ReturnsAsync((IResult)null);
        _mockMapper.Setup(m => m.Map<Purchase>(It.IsAny<PurchaseRequestDto>())).Returns(purchase);
        _mockPurchaseRepository
            .Setup(r => r.AddAsync(It.IsAny<Purchase>(), It.IsAny<CoursePurchaseDetails>()))
            .ReturnsAsync(purchase);

        var result = await _purchaseMakingService.MakeCoursePurchase(dto, Guid.NewGuid());

        _mockTokenService.Verify(x => x.GetToken(It.IsAny<Purchase>()));
        _mockPurchaseRepository.Verify(x => x.UpdateAsync(It.IsAny<Purchase>()));
        _mockPurchaseCreatedProducer.Verify(x => x.PublishAsync(It.IsAny<Purchase>()));
        Assert.IsType<Ok>(result);
    }

    [Fact]
    public async Task GetCertificatePreparedPurchase_CertificateNotFound_ReturnsNotFound()
    {
        _mockCourseRepository.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync((Course)null);

        var result = await _purchaseMakingService.GetCertificatePreparedPurchase(1);

        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetCertificatePreparedPurchase_CertificateFound_ReturnsOk()
    {
        var course = new Course
        {
            Id = 1,
            Title = "Test Course",
            HasCertificates = true
        };
        _mockCourseRepository.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(course);
        _mockMapper
            .Setup(m => m.Map<PreparedPurchaseResponseDto>(It.IsAny<Course>()))
            .Returns(new PreparedPurchaseResponseDto(It.IsAny<string>(), It.IsAny<decimal>()));

        var result = await _purchaseMakingService.GetCertificatePreparedPurchase(1);

        Assert.IsType<Ok<PreparedPurchaseResponseDto>>(result);
    }

    [Fact]
    public async Task MakeCertificatePurchase_ValidationFails_ReturnsErrorResult()
    {
        var dto = new PurchaseRequestDto(
            It.IsAny<Guid>(),
            It.IsAny<decimal>(),
            It.IsAny<CardInfoDto>(),
            It.IsAny<PurchaseType>(),
            It.IsAny<int>(),
            It.IsAny<int>()
        );
        _mockValidatePurchaseService
            .Setup(x =>
                x.ValidatePurchase(It.IsAny<PurchaseRequestDto>(), It.IsAny<PurchaseType>())
            )
            .ReturnsAsync(Results.BadRequest("Validation Failed"));

        var result = await _purchaseMakingService.MakeCertificatePurchase(dto, Guid.NewGuid());

        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
    public async Task GetSubscriptionPreparedPurchase_SubscriptionNotFound_ReturnsNotFound()
    {
        _mockSubscriptionRepository
            .Setup(x => x.GetAsync(It.IsAny<int>(), false))
            .ReturnsAsync((Subscription)null);

        var result = await _purchaseMakingService.GetSubscriptionPreparedPurchase(1);

        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetSubscriptionPreparedPurchase_SubscriptionFreePurchaseAttempt_ReturnsForbid()
    {
        var freeSubscription = new Subscription { Id = (int)SubscriptionType.Free };
        _mockSubscriptionRepository
            .Setup(x => x.GetAsync(freeSubscription.Id, false))
            .ReturnsAsync(freeSubscription);

        var result = await _purchaseMakingService.GetSubscriptionPreparedPurchase(
            (int)SubscriptionType.Free
        );

        Assert.IsType<ForbidHttpResult>(result);
    }

    [Fact]
    public async Task GetSubscriptionPreparedPurchase_SubscriptionFound_ReturnsOk()
    {
        var subscription = new Subscription() { Id = 2, };
        _mockSubscriptionRepository.Setup(x => x.GetAsync(2, false)).ReturnsAsync(subscription);
        _mockMapper
            .Setup(m => m.Map<PreparedPurchaseResponseDto>(It.IsAny<Subscription>()))
            .Returns(new PreparedPurchaseResponseDto(It.IsAny<string>(), It.IsAny<decimal>()));

        var result = await _purchaseMakingService.GetSubscriptionPreparedPurchase(2);

        Assert.IsType<Ok<PreparedPurchaseResponseDto>>(result);
    }

    [Fact]
    public async Task MakeSubscriptionPurchase_ValidationFails_ReturnsErrorResult()
    {
        var dto = new PurchaseRequestDto(
            It.IsAny<Guid>(),
            It.IsAny<decimal>(),
            It.IsAny<CardInfoDto>(),
            It.IsAny<PurchaseType>(),
            It.IsAny<int>(),
            It.IsAny<int>()
        );
        _mockValidatePurchaseService
            .Setup(x =>
                x.ValidatePurchase(It.IsAny<PurchaseRequestDto>(), It.IsAny<PurchaseType>())
            )
            .ReturnsAsync(Results.BadRequest("Validation Failed"));

        var result = await _purchaseMakingService.MakeSubscriptionPurchase(dto, Guid.NewGuid());

        Assert.IsType<BadRequest<string>>(result);
    }
}
