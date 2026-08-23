namespace ShNotes.UseCases.Users;

public sealed class UserAlreadyCreatedException : UseCaseException
{
    private const string _message = """
            Пользователь с таким username уже существует!
        """;

    public UserAlreadyCreatedException()
        : base(_message) { }
}
