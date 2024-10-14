using Purchases.Services;

namespace Tests.Purchases.UnitTests.ServiceTests;

public class JwtTokenServiceTests
{
    private readonly string _secretKey = "SecretKey123";
    private readonly JwtTokenService _tokenService;

    public JwtTokenServiceTests()
    {
        _tokenService = new JwtTokenService(_secretKey);
    }

    [Fact]
    public void GenerateToken_WithValidClaims_ReturnsValidToken()
    {
        // Arrange
        var claims = new Dictionary<string, object?>
        {
            { "sub", "1234567890" },
            { "name", "John Doe" },
            { "admin", true }
        };

        // Act
        var token = _tokenService.GenerateToken(claims);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public void GenerateToken_WithEmptyClaims_ReturnsValidToken()
    {
        // Arrange
        var claims = new Dictionary<string, object?>();

        // Act
        var token = _tokenService.GenerateToken(claims);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public void DecodeToken_WithValidToken_ReturnsDecodedClaims()
    {
        // Arrange
        var expectedClaims = new Dictionary<string, object?>
        {
            { "sub", "1234567890" },
            { "name", "John Doe" },
            { "admin", true }
        };
        var token = _tokenService.GenerateToken(expectedClaims);

        // Act
        var decoded = _tokenService.DecodeToken(expectedClaims, token);

        // Assert
        Assert.Equal($"{{\"sub\":\"1234567890\",\"name\":\"John Doe\",\"admin\":true}}", decoded);
    }
}
