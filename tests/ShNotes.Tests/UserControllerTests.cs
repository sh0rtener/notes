using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using ShNotes.UseCases.Users;
using ShNotes.UseCases.Users.GetUser;
using ShNotes.WebApi.Common;
using ShNotes.WebApi.Controllers;
using ShNotes.WebApi.Jwt;
using ShNotes.WebApi.Models.Notes;

namespace ShNotes.Tests;

public class UserControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly JwtService _jwtService;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();

        var configuration = new JwtConfiguration
        {
            Key = "975c8437e38ef3ef35f0bab6cc236595432b1f63861be8e22f2ecd0def02c397",
            Issuer = "localhost",
            Audience = "localhost",
            AccessTokenExpires = 15,
            RefreshTokenExpires = 10080
        };

        _jwtService = new JwtService(Options.Create(configuration));
        _controller = new UserController(_mediatorMock.Object, _jwtService);
    }

    [Fact]
    public async Task RefreshToken_WithValidToken_ReturnsNewTokens()
    {
        // Arrange
        var userId = 42;
        var userName = "testuser";
        var refreshClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Thumbprint, userName)
        };

        var initialRefreshToken = _jwtService.CreateToken(
            refreshClaims,
            DateTime.UtcNow.AddMinutes(10080)
        );
        var initialRefreshTokenString = _jwtService.WriteToken(initialRefreshToken);

        _mediatorMock
            .Setup(m => m.Send(
                It.Is<GetUserQuery>(q => q.UserId == userId),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new UserDto
            {
                Id = userId,
                Name = userName,
                CreatedAt = DateTime.UtcNow
            });

        var request = new RefreshTokenRequest
        {
            RefreshToken = initialRefreshTokenString
        };

        // Act
        var result = await _controller.RefreshToken(request, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var apiResponse = Assert.IsType<ApiResponse<SignInResponse>>(okResult.Value);
        Assert.NotNull(apiResponse.Data);
        Assert.False(string.IsNullOrWhiteSpace(apiResponse.Data.AccessToken));
        Assert.False(string.IsNullOrWhiteSpace(apiResponse.Data.RefreshToken));

        // Verify the new tokens are valid and have correct claims
        var newAccessPrincipal = _jwtService.GetClaimsPrincipal(apiResponse.Data.AccessToken);
        Assert.Equal(userId.ToString(), newAccessPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var newRefreshPrincipal = _jwtService.GetClaimsPrincipal(apiResponse.Data.RefreshToken);
        Assert.Equal(userId.ToString(), newRefreshPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        Assert.Equal(userName, newRefreshPrincipal.FindFirst(ClaimTypes.Thumbprint)?.Value);
    }

    [Fact]
    public async Task RefreshToken_WithInvalidToken_ThrowsInvalidDataException()
    {
        // Arrange
        var request = new RefreshTokenRequest
        {
            RefreshToken = "invalid-token"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidDataException>(
            () => _controller.RefreshToken(request, CancellationToken.None)
        );
    }

    [Fact]
    public async Task RefreshToken_WhenUserNotFound_ThrowsUserWasntFoundException()
    {
        // Arrange
        var userId = 999;
        var refreshClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Thumbprint, "nonexistent")
        };

        var initialRefreshToken = _jwtService.CreateToken(
            refreshClaims,
            DateTime.UtcNow.AddMinutes(10080)
        );
        var initialRefreshTokenString = _jwtService.WriteToken(initialRefreshToken);

        _mediatorMock
            .Setup(m => m.Send(
                It.Is<GetUserQuery>(q => q.UserId == userId),
                It.IsAny<CancellationToken>()
            ))
            .ThrowsAsync(new UserWasntFoundException());

        var request = new RefreshTokenRequest
        {
            RefreshToken = initialRefreshTokenString
        };

        // Act & Assert
        await Assert.ThrowsAsync<UserWasntFoundException>(
            () => _controller.RefreshToken(request, CancellationToken.None)
        );
    }
}

