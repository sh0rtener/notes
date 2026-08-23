using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShNotes.UseCases.Users.CreateUser;
using ShNotes.UseCases.Users.GetUser;
using ShNotes.UseCases.Users.SignIn;
using ShNotes.WebApi.Common;
using ShNotes.WebApi.Jwt;
using ShNotes.WebApi.Models.Notes;

namespace ShNotes.WebApi.Controllers;

[ApiController]
[Route("users")]
public sealed class UserController : ControllerBase
{
    private int UserId =>
        int.Parse(
            User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "0"
        );
    private readonly IMediator _mediator;
    private readonly JwtService _jwtService;

    public UserController(IMediator mediator, IOptions<JwtConfiguration> options)
    {
        _mediator = mediator;
        _jwtService = new JwtService(options);
    }

    [Authorize]
    [HttpGet()]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetUserQuery() { UserId = int.Parse(UserId.ToString()) },
            cancellationToken
        );
        return this.SendOkResult(result);
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn(
        [FromBody] SignInRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await _mediator.Send(
            new SignInCommand() { Username = request.Username, Password = request.Password },
            cancellationToken
        );

        var accessClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, result.Id.ToString()),
        };

        var refreshClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, result.Id.ToString()),
            new Claim(ClaimTypes.Thumbprint, result.Name.ToString()),
        };

        var accessToken = _jwtService.CreateToken(
            accessClaims,
            DateTime.Now.AddMinutes(_jwtService.Configuration.AccessTokenExpires)
        );
        var refreshToken = _jwtService.CreateToken(
            refreshClaims,
            DateTime.Now.AddMinutes(_jwtService.Configuration.RefreshTokenExpires)
        );

        return this.SendOkResult(
            new SignInResponse()
            {
                AccessToken = _jwtService.WriteToken(accessToken),
                RefreshToken = _jwtService.WriteToken(refreshToken),
                ExpiresAt = DateTime.Now.AddMinutes(_jwtService.Configuration.AccessTokenExpires),
                RefreshExpiresAt = DateTime.Now.AddMinutes(
                    _jwtService.Configuration.RefreshTokenExpires
                ),
            }
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserRequest user,
        CancellationToken cancellationToken
    )
    {
        var result = await _mediator.Send(
            new CreateAccountCommand() { Username = user.Username, Password = user.Password },
            cancellationToken
        );

        return this.SendOkResult(result);
    }
}
