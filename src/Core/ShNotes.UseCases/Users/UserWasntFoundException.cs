namespace ShNotes.UseCases.Users;

public sealed class UserWasntFoundException : UseCaseException
{
    private const string _message = """
            Пользователь не найден
        """;

    public UserWasntFoundException()
        : base(_message) { }
}
