using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShNotes.Core.Users;

namespace ShNotes.Data.EntityFramework;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly IScriptProvider _scriptProvider;
    private readonly IMapper _mapper;

    public UserRepository(AppDbContext context, IScriptProvider scriptProvider, IMapper mapper)
    {
        _context = context;
        _scriptProvider = scriptProvider;
        _mapper = mapper;
    }

    public async Task<User> Create(User user, CancellationToken cancellationToken = default)
    {
        var parameters = _scriptProvider.CreateUserParameters(user);
        var userId = (
            await _context
                .Database.SqlQueryRaw<int>(_scriptProvider.CreateUser, parameters.ToArray())
                .ToListAsync(cancellationToken)
        ).First();
        user.Id = userId;

        return user;
    }

    public async Task<User?> Get(int id, CancellationToken cancellationToken = default)
    {
        var parameters = _scriptProvider.GetUserParameters(id);
        var user = (
            await _context
                .Users.FromSqlRaw(_scriptProvider.GetUser, parameters.ToArray())
                .ToListAsync(cancellationToken)
        ).FirstOrDefault();

        if (user is null)
            return null;

        return _mapper.Map<User>(user);
    }

    public async Task<User?> Get(string username, CancellationToken cancellationToken = default)
    {
        var parameters = _scriptProvider.GetUserByUsernameParameters(username);
        var user = (
            await _context
                .Users.FromSqlRaw(_scriptProvider.GetUserByUsername, parameters.ToArray())
                .ToListAsync(cancellationToken)
        ).FirstOrDefault();

        if (user is null)
            return null;

        return _mapper.Map<User>(user);
    }

    public async Task<bool> IsExists(string username, CancellationToken cancellationToken = default)
    {
        var parameters = _scriptProvider.IsExistsUserParameters(username);
        var result = (
            await _context
                .Database.SqlQueryRaw<int>(_scriptProvider.IsExistsUser, parameters.ToArray())
                .ToListAsync(cancellationToken)
        );

        return result is not null && result.Count >= 1;
    }

    public Task Remove(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
