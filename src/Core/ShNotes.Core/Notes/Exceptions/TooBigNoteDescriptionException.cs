namespace ShNotes.Core.Notes.Exceptions;

public sealed class TooBigNoteDescriptionException : CoreException
{
    private const string _message = "Описание заметки должно быть менее 1000 символов";

    public TooBigNoteDescriptionException()
        : base(_message) { }
}
