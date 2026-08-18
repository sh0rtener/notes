namespace ShNotes.Core.Notes;

public interface INoteRepository
{
    Task<Note> Get(int noteId, CancellationToken cancellationToken = default);
    Task<int> Add(Note note, CancellationToken cancellationToken = default);
    Task<Note> Update(int id, Note note, CancellationToken cancellationToken = default);
    Task Remove(int id, CancellationToken cancellationToken = default);
}
