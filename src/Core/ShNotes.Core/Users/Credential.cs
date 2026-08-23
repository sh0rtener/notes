using ShNotes.Core.Auth;
using ShNotes.Core.Users.Exceptions;

namespace ShNotes.Core.Users;

public sealed class Credential : Entity<int>
{
    public string PasswordHash { get; private set; } = null!;
    public string PasswordSalt { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    private Pbkd2Sha256Alghoritm cypherService = new Pbkd2Sha256Alghoritm();

    public Credential() { }

    public Credential(string password)
    {
        CreatePassword(password);
    }

    public void ChangePassword(string oldPassword, string password)
    {
        ThrowIsInvalidate(oldPassword);
        CreatePassword(password);
    }

    public void CreatePassword(string password)
    {
        var encryptedPair = cypherService.Crypt(password);
        PasswordHash = encryptedPair.Hash;
        PasswordSalt = encryptedPair.Salt;
    }

    public void ThrowIsInvalidate(string password)
    {
        var isValid = cypherService.Validate(password, new(PasswordHash, PasswordSalt));

        if (!isValid)
            throw new InvalidPasswordException();
    }
}
