using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Purchases.Data.Abstractions;
using Purchases.Data.Models;
using Purchases.Dtos;
using Purchases.Dtos.Requests;
using Purchases.Dtos.Responses;
using Purchases.Services;
using Purchases.Services.Abstractions;

namespace Tests.Purchases.UnitTests.ServiceTests;

public class PurchaseServiceTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPurchaseRepository> _mockPurchaseRepository;
    private readonly Mock<IPurchaseTokenService> _mockTokenService;
    private readonly PurchaseService _purchaseService;

    public PurchaseServiceTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPurchaseRepository = new Mock<IPurchaseRepository>();
        _mockTokenService = new Mock<IPurchaseTokenService>();
        _purchaseService = new PurchaseService(
            _mockMapper.Object,
            _mockUserRepository.Object,
            _mockPurchaseRepository.Object,
            _mockTokenService.Object
        );
    }

    [Fact]
    public async Task GetPurchasesByUser_UserNotFound_ReturnsNotFoundResult()
    {
        _mockUserRepository
            .Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync((User)null!);

        var result = await _purchaseService.GetPurchasesByUser(Guid.NewGuid());

        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetPurchasesByUser_UserFound_ReturnsOkResult()
    {
        var purchases = new List<Purchase> { new Purchase { Price = 1 } };
        var user = new User { Purchases = purchases };
        _mockUserRepository
            .Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(user);
        _mockMapper
            .Setup(mapper => mapper.Map<List<PurchaseInfoResponseDto>>(It.IsAny<List<Purchase>>()))
            .Returns(new List<PurchaseInfoResponseDto>());

        var result = await _purchaseService.GetPurchasesByUser(Guid.NewGuid());

        Assert.IsType<Ok>(result);
    }

    [Fact]
    public async Task Verify_BadPurchaseOrUser_ReturnsBadRequestResult()
    {
        _mockTokenService
            .Setup(service => service.ConvertToPurchaseTokenInfo(It.IsAny<string>()))
            .Returns(new PurchaseTokenInfo { UserId = Guid.NewGuid(), Id = 1 });
        _mockUserRepository
            .Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync((User)null!);

        var result = await _purchaseService.Verify(
            new VerifyTokenRequestDto { Token = "dummy-token" }
        );

        Assert.IsType<BadRequest>(result);
    }

    [Fact]
    public async Task Verify_TokenDoesNotMatch_ReturnsForbiddenResult()
    {
        var purchaseTokenInfo = new PurchaseTokenInfo { UserId = Guid.NewGuid(), Id = 1 };
        _mockTokenService
            .Setup(service => service.ConvertToPurchaseTokenInfo(It.IsAny<string>()))
            .Returns(purchaseTokenInfo);
        _mockUserRepository
            .Setup(repo =>
                repo.GetAsync(purchaseTokenInfo.UserId, It.IsAny<bool>(), It.IsAny<bool>())
            )
            .ReturnsAsync(new User());
        _mockPurchaseRepository
            .Setup(repo => repo.GetAsync(purchaseTokenInfo.Id))
            .ReturnsAsync(new Purchase { Token = "another-token" });

        var result = await _purchaseService.Verify(
            new VerifyTokenRequestDto { Token = "dummy-token" }
        );

        Assert.IsType<ForbidHttpResult>(result);
    }

    [Fact]
    public async Task Verify_TokenMatches_ReturnsOkResult()
    {
        var purchaseTokenInfo = new PurchaseTokenInfo { UserId = Guid.NewGuid(), Id = 1 };
        _mockTokenService
            .Setup(service => service.ConvertToPurchaseTokenInfo(It.IsAny<string>()))
            .Returns(purchaseTokenInfo);
        _mockUserRepository
            .Setup(repo =>
                repo.GetAsync(purchaseTokenInfo.UserId, It.IsAny<bool>(), It.IsAny<bool>())
            )
            .ReturnsAsync(new User());
        _mockPurchaseRepository
            .Setup(repo => repo.GetAsync(purchaseTokenInfo.Id))
            .ReturnsAsync(new Purchase { Token = "matched-token" });

        var result = await _purchaseService.Verify(
            new VerifyTokenRequestDto { Token = "matched-token" }
        );

        Assert.IsType<Ok>(result);
    }
}
