namespace ShNotes.Core.Users.Exceptions;

public sealed class InvalidPasswordException : CoreException
{
    private const string _message = "Неверные данные пользователя";
    public InvalidPasswordException() : base(_message) { }
}