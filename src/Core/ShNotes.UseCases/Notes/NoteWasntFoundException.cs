namespace ShNotes.UseCases.Notes;

public sealed class NoteWasntFoundException : UseCaseException
{
    private const string _message = """
            Заметка с указанным идентификатором не найдена!
        """;

    public NoteWasntFoundException()
        : base(_message) { }
}
