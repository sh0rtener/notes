using System.Text.RegularExpressions;
using ShNotes.Core.Users.Exceptions;

namespace ShNotes.Core.Users;

public sealed class User : Entity<int>
{
    public const string _nameRegex = @"^(?=.{3,15}$)[^*\\/'""^<>:|?]+$";
    public string Name { get; private set; }
    public Credential Credential { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User(string name, Credential credential)
    {
        if (string.IsNullOrEmpty(name) || !Regex.IsMatch(name, _nameRegex))
            throw new InvalidUserNameException();

        if (credential == null)
            throw new InvalidCredentialsException();

        Name = name;
        CreatedAt = DateTime.UtcNow;
        Credential = credential;
    }
}
