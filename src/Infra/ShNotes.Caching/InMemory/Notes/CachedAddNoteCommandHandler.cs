using MediatR;
using ShNotes.UseCases.Notes.AddNote;

namespace ShNotes.Caching.InMemory.Notes;

public sealed class CachedAddNoteCommandHandler : IRequestHandler<AddNoteCommand, int>
{
    private readonly AddNoteCommandHandler _inner;

    public CachedAddNoteCommandHandler(AddNoteCommandHandler inner)
    {
        _inner = inner;
    }

    public async Task<int> Handle(AddNoteCommand request, CancellationToken cancellationToken)
    {
        await CacheInvalidator.GetInstance().AddCacheCts.CancelAsync();
        return await _inner.Handle(request, cancellationToken);
    }
}
