using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Purchases.Data.Abstractions;
using Purchases.Data.Enums;
using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;
using Purchases.Dtos;
using Purchases.Dtos.Requests;
using Purchases.Services;

namespace Tests.Purchases.UnitTests.ServiceTests;

public class ValidatePurchaseServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository = new();
    private readonly Mock<ICourseRepository> _mockCourseRepository = new();
    private readonly Mock<ISubscriptionRepository> _mockSubscriptionRepository = new();
    private readonly Mock<IUserCourseInfoRepository> _mockUserCourseInfoRepository = new();
    private readonly ValidatePurchaseService _validatePurchaseService;

    public ValidatePurchaseServiceTests()
    {
        _validatePurchaseService = new ValidatePurchaseService(
            _mockUserRepository.Object,
            _mockCourseRepository.Object,
            _mockSubscriptionRepository.Object,
            _mockUserCourseInfoRepository.Object
        );
    }

    [Fact]
    public async Task ValidatePurchase_InvalidType_ThrowsArgumentOutOfRangeException()
    {
        var dto = new PurchaseRequestDto(
            It.IsAny<Guid>(),
            It.IsAny<decimal>(),
            It.IsAny<CardInfoDto>(),
            (PurchaseType)999,
            It.IsAny<int>(),
            It.IsAny<int>()
        );
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => _validatePurchaseService.ValidatePurchase(dto, (PurchaseType)999)
        );
    }

    [Theory]
    [InlineData(PurchaseType.Course)]
    [InlineData(PurchaseType.Certificate)]
    [InlineData(PurchaseType.Subscription)]
    public async Task ValidatePurchase_ValidTypes_DoesNotThrow(PurchaseType validType)
    {
        var dto = new PurchaseRequestDto(
            It.IsAny<Guid>(),
            It.IsAny<decimal>(),
            It.IsAny<CardInfoDto>(),
            validType,
            It.IsAny<int>(),
            It.IsAny<int>()
        );

        IResult? result = await _validatePurchaseService.ValidatePurchase(dto, validType);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task ValidateCoursePurchase_MissingCourseId_ReturnsBadRequest()
    {
        var dto = new PurchaseRequestDto(
            It.IsAny<Guid>(),
            It.IsAny<decimal>(),
            It.IsAny<CardInfoDto>(),
            PurchaseType.Course,
            null,
            It.IsAny<int>()
        );

        var result = await _validatePurchaseService.ValidatePurchase(dto, PurchaseType.Course);

        Assert.IsType<BadRequest>(result);
    }

    [Fact]
    public async Task ValidateCoursePurchase_UserOrCourseNotFound_ReturnsNotFound()
    {
        var dto = new PurchaseRequestDto(
            new Guid(),
            It.IsAny<decimal>(),
            It.IsAny<CardInfoDto>(),
            PurchaseType.Course,
            1,
            It.IsAny<int>()
        );

        _mockUserRepository
            .Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync((User)null);
        _mockCourseRepository.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync((Course)null);

        var result = await _validatePurchaseService.ValidatePurchase(dto, PurchaseType.Course);

        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task ValidateCoursePurchase_AlreadyBought_ReturnsForbidden()
    {
        var dto = new PurchaseRequestDto(
            It.IsAny<Guid>(),
            It.IsAny<decimal>(),
            It.IsAny<CardInfoDto>(),
            PurchaseType.Course,
            1,
            It.IsAny<int>()
        );
        var user = new User
        {
            Purchases = new List<Purchase>
            {
                new Purchase
                {
                    PurchaseDetails = new CoursePurchaseDetails
                    {
                        CourseId = 1,
                        PurchaseType = PurchaseType.Course
                    }
                }
            }
        };
        var course = new Course { Id = 1 };
        _mockUserRepository
            .Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(user);
        _mockCourseRepository.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync(course);

        var result = await _validatePurchaseService.ValidatePurchase(dto, PurchaseType.Course);

        Assert.IsType<ForbidHttpResult>(result);
    }

    [Fact]
    public async Task ValidateCoursePurchase_PriceMismatch_ReturnsBadRequest()
    {
        var dto = new PurchaseRequestDto(
            It.IsAny<Guid>(),
            500,
            It.IsAny<CardInfoDto>(),
            PurchaseType.Course,
            1,
            It.IsAny<int>()
        );
        var user = new User { Id = new Guid(), Purchases = new List<Purchase>() };
        var course = new Course { Id = 1, Price = 1000 };
        _mockUserRepository
            .Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(user);
        _mockCourseRepository.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync(course);

        var result = await _validatePurchaseService.ValidatePurchase(dto, PurchaseType.Course);

        Assert.IsType<BadRequest>(result);
    }

    [Fact]
    public async Task ValidateCoursePurchase_Valid_ReturnsNull()
    {
        var dto = new PurchaseRequestDto(
            It.IsAny<Guid>(),
            1000,
            It.IsAny<CardInfoDto>(),
            PurchaseType.Course,
            1,
            It.IsAny<int>()
        );
        var user = new User { Id = new Guid(), Purchases = new List<Purchase>() };
        var course = new Course { Id = 1, Price = 1000 };
        _mockUserRepository
            .Setup(r => r.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(user);
        _mockCourseRepository.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync(course);

        var result = await _validatePurchaseService.ValidatePurchase(dto, PurchaseType.Course);

        Assert.Null(result);
    }
}
