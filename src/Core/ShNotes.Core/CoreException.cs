namespace ShNotes.Core;

public abstract class CoreException : Exception
{
    public CoreException(string message)
        : base(message) { }
}
