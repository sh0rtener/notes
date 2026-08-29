using System.Security.Claims;
using Microsoft.Extensions.Options;
using ShNotes.WebApi.Jwt;

namespace ShNotes.Tests;

public class JwtServiceTests
{
    private readonly JwtService _jwtService;
    private readonly JwtConfiguration _configuration;

    public JwtServiceTests()
    {
        _configuration = new JwtConfiguration
        {
            Key = "975c8437e38ef3ef35f0bab6cc236595432b1f63861be8e22f2ecd0def02c397",
            Issuer = "localhost",
            Audience = "localhost",
            AccessTokenExpires = 15,
            RefreshTokenExpires = 10080
        };

        _jwtService = new JwtService(Options.Create(_configuration));
    }

    [Fact]
    public void CreateToken_And_GetClaimsPrincipal_ShouldReturnValidClaims()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "42"),
            new(ClaimTypes.Thumbprint, "testuser")
        };

        // Act
        var token = _jwtService.CreateToken(claims, DateTime.UtcNow.AddMinutes(10));
        var tokenString = _jwtService.WriteToken(token);
        var principal = _jwtService.GetClaimsPrincipal(tokenString);

        // Assert
        Assert.NotNull(principal);
        var idClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var nameClaim = principal.FindFirst(ClaimTypes.Thumbprint)?.Value;

        Assert.Equal("42", idClaim);
        Assert.Equal("testuser", nameClaim);
    }

    [Fact]
    public void GetClaimsPrincipal_WithEmptyToken_ShouldThrowInvalidDataException()
    {
        // Act & Assert
        Assert.Throws<InvalidDataException>(() => _jwtService.GetClaimsPrincipal(string.Empty));
    }

    [Fact]
    public void GetClaimsPrincipal_WithInvalidTokenString_ShouldThrowInvalidDataException()
    {
        // Act & Assert
        Assert.Throws<InvalidDataException>(() => _jwtService.GetClaimsPrincipal("not-a-valid-jwt-token"));
    }

    [Fact]
    public void GetClaimsPrincipal_WithExpiredToken_ShouldThrowInvalidDataException()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "42")
        };

        var expiredToken = _jwtService.CreateToken(claims, DateTime.UtcNow.AddMinutes(-5));
        var tokenString = _jwtService.WriteToken(expiredToken);

        // Act & Assert
        Assert.Throws<InvalidDataException>(() => _jwtService.GetClaimsPrincipal(tokenString));
    }

    [Fact]
    public void GetClaimsPrincipal_WithDifferentKey_ShouldThrowInvalidDataException()
    {
        // Arrange
        var otherConfig = new JwtConfiguration
        {
            Key = "1111111111111111111111111111111111111111111111111111111111111111",
            Issuer = "localhost",
            Audience = "localhost",
            AccessTokenExpires = 15,
            RefreshTokenExpires = 10080
        };
        var otherJwtService = new JwtService(Options.Create(otherConfig));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "42")
        };
        var token = otherJwtService.CreateToken(claims, DateTime.UtcNow.AddMinutes(10));
        var tokenString = otherJwtService.WriteToken(token);

        // Act & Assert
        Assert.Throws<InvalidDataException>(() => _jwtService.GetClaimsPrincipal(tokenString));
    }

    [Fact]
    public void GenerateTokenResponse_ShouldReturnValidSignInResponse()
    {
        // Act
        var response = _jwtService.GenerateTokenResponse(100, "bob");

        // Assert
        Assert.NotNull(response);
        Assert.False(string.IsNullOrWhiteSpace(response.AccessToken));
        Assert.False(string.IsNullOrWhiteSpace(response.RefreshToken));
        Assert.True(response.ExpiresAt > DateTime.Now);
        Assert.True(response.RefreshExpiresAt > response.ExpiresAt);

        var accessPrincipal = _jwtService.GetClaimsPrincipal(response.AccessToken);
        Assert.Equal("100", accessPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var refreshPrincipal = _jwtService.GetClaimsPrincipal(response.RefreshToken);
        Assert.Equal("100", refreshPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        Assert.Equal("bob", refreshPrincipal.FindFirst(ClaimTypes.Thumbprint)?.Value);
    }
}

