using MediatR;
using ShNotes.Core.Users;

namespace ShNotes.UseCases.Users.GetUser;

public sealed class GetUserQuery : IRequest<UserDto>
{
    public required int UserId { get; set; }
}

public sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Get(request.UserId, cancellationToken);

        if (user is null)
            throw new UserWasntFoundException();

        return new()
        {
            Id = user.Id,
            Name = user.Name,
            CreatedAt = user.CreatedAt,
        };
    }
}
