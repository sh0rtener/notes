namespace ShNotes.UseCases.Notes;

public sealed class NotePermissionException : UseCaseException
{
    private const string _message = """
        Данная заметка не является заметкой пользователя
    """;
    public NotePermissionException() : base(_message)
    {
    }
}