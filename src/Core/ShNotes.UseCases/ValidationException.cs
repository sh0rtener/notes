namespace ShNotes.UseCases;

public sealed class ValidationException : Exception
{
    public Dictionary<string, string[]> Errors { get; }

    public ValidationException()
        : base("Ошибка валидации модели. Смотрите подробнее ошибки и условия")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(Dictionary<string, string[]> failures)
        : this()
    {
        Errors = failures;
    }
}
