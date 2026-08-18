namespace ShNotes.Core.Notes.Exceptions;

public sealed class InvalidNoteNameException : CoreException
{
    private const string _message =
        "Неверно задано имя заметки. Имя должно быть более 3 символов и менее 25, а также не содержать следующих символов: (*,\\,/,',\",^,<,>,:,|,?)";

    public InvalidNoteNameException()
        : base(_message) { }
}
