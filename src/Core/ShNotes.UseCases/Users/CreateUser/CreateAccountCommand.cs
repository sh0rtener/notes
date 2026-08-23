using AutoMapper;
using MediatR;
using ShNotes.Core.Users;

namespace ShNotes.UseCases.Users.CreateUser;

public sealed class CreateAccountCommand : IRequest<UserDto>
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public sealed class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, UserDto>
{
    private IMapper _mapper;
    private IUserRepository _userRepository;

    public CreateAccountCommandHandler(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(
        CreateAccountCommand request,
        CancellationToken cancellationToken
    )
    {
        if (await _userRepository.IsExists(request.Username, cancellationToken))
            throw new UserAlreadyCreatedException();

        var user = new User(request.Username, new Credential(request.Password));
        var createdUser = await _userRepository.Create(user, cancellationToken);

        return _mapper.Map<UserDto>(createdUser);
    }
}
