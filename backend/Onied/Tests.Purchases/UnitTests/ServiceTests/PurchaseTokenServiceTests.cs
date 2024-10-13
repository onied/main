using System.Text.Json;
using MassTransit.Data.Enums;
using Moq;
using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;
using Purchases.Dtos;
using Purchases.Services;
using Purchases.Services.Abstractions;

namespace Tests.Purchases.UnitTests.ServiceTests;

public class PurchaseTokenServiceTests
{
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock = new();

    [Fact]
    public void GetToken_CoursePurchaseDetails_ReturnsCorrectToken()
    {
        // Arrange
        var purchase = new Purchase
        {
            Id = 1,
            UserId = Guid.NewGuid(),
            PurchaseDetails = new CoursePurchaseDetails { CourseId = 1 }
        };
        var claims = new Dictionary<string, object?>()
        {
            { "Id", purchase.Id },
            { "PurchaseType", purchase.PurchaseDetails.PurchaseType },
            { "UserId", purchase.UserId },
            {
                "CourseId",
                purchase.PurchaseDetails is CoursePurchaseDetails pd1
                    ? pd1.CourseId
                    : (purchase.PurchaseDetails as CertificatePurchaseDetails)?.CourseId
            },
            {
                "SubscriptionId",
                (purchase.PurchaseDetails as SubscriptionPurchaseDetails)?.SubscriptionId
            },
            { "Expires", (purchase.PurchaseDetails as SubscriptionPurchaseDetails)?.EndDate }
        };
        var expectedToken = "expectedToken";
        _jwtTokenServiceMock.Setup(x => x.GenerateToken(claims)).Returns(expectedToken);

        var service = new PurchaseTokenService(_jwtTokenServiceMock.Object);

        // Act
        var result = service.GetToken(purchase);

        // Assert
        Assert.Equal(expectedToken, result);
    }

    [Fact]
    public void ConvertToPurchaseTokenInfo_ValidToken_ReturnsDeserializedInfo()
    {
        // Arrange
        var token = "validToken";
        var expectedInfo = new PurchaseTokenInfo
        {
            Id = 1,
            PurchaseType = PurchaseType.Course,
            UserId = Guid.NewGuid(),
            CourseId = 1,
            SubscriptionId = 0,
            Expires = DateTime.UtcNow.AddHours(1)
        };
        _jwtTokenServiceMock
            .Setup(x => x.DecodeToken(It.IsAny<Dictionary<string, object?>>(), token))
            .Returns(JsonSerializer.Serialize(expectedInfo));

        var service = new PurchaseTokenService(_jwtTokenServiceMock.Object);

        // Act
        var result = service.ConvertToPurchaseTokenInfo(token);

        // Assert
        Assert.Equivalent(expectedInfo, result);
    }
}
