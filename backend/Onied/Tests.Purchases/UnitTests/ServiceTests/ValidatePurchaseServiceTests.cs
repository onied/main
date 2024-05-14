using Microsoft.AspNetCore.Http;
using Moq;
using Purchases.Data.Abstractions;
using Purchases.Data.Enums;
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
}
