namespace ShNotes.Core.Users.Exceptions;

public sealed class InvalidUserNameException : CoreException
{
    private const string _message = """
        Неверно передан параметр Name в сущность Users.
        Name не может быть менее 3 символов и более 15, а также не содержать следующих символов: (*,\\,/,',\",^,<,>,:,|,?)
    """;
    public InvalidUserNameException() : base(_message)
    {
    }
}