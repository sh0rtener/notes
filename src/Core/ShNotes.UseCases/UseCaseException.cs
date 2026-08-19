namespace ShNotes.UseCases;

public class UseCaseException : Exception
{
    public UseCaseException(string message)
        : base(message) { }
}
