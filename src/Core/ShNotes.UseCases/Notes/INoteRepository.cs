namespace ShNotes.UseCases.Notes;

public record GetNoteFilter(
    string? Name,
    string? Status,
    DateTime? DateFrom,
    int Limit = 100,
    int Offset = 0
);

public interface INoteRepository
{
    Task<IEnumerable<ShortNoteDto>> Get(
        GetNoteFilter filter,
        CancellationToken cancellationToken = default
    );
}
