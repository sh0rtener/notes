namespace ShNotes.Core.Users.Exceptions;

public sealed class InvalidCredentialsException : CoreException
{
    private const string _message = "Переданы неверные секреты пользователя";

    public InvalidCredentialsException()
        : base(_message) { }
}
