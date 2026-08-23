namespace ShNotes.Core.Users;

public interface IUserRepository
{
    Task<User?> Get(int id, CancellationToken cancellationToken = default);
    Task<User?> Get(string username, CancellationToken cancellationToken = default);
    Task<bool> IsExists(string username, CancellationToken cancellationToken = default);
    Task<User> Create(User user, CancellationToken cancellationToken = default);
    Task Remove(int id, CancellationToken cancellationToken = default);
}
