using MediatR;
using ShNotes.Core.Users;

namespace ShNotes.UseCases.Users.SignIn;

public sealed class SignInCommand : IRequest<UserDto>
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public sealed class SignInCommandHandler : IRequestHandler<SignInCommand, UserDto>
{
    private readonly IUserRepository _userRepository;

    public SignInCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Get(request.Username, cancellationToken);

        if (user is null)
            throw new UserWasntFoundException();

        user.Credential.ThrowIsInvalidate(request.Password);

        return new()
        {
            Id = user.Id,
            Name = user.Name,
            CreatedAt = user.CreatedAt,
        };
    }
}
