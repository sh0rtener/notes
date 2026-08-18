namespace ShNotes.Core.Notes.Exceptions;

public sealed class CannotSetNullDescriptionException : CoreException
{
    private const string _message = "Описание не может быть NULL!";

    public CannotSetNullDescriptionException()
        : base(_message) { }
}
