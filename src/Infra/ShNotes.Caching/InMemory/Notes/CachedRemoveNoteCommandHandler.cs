using MediatR;
using Microsoft.Extensions.Caching.Memory;
using ShNotes.UseCases.Notes.RemoveNote;

namespace ShNotes.Caching.InMemory.Notes;

public sealed class CachedRemoveNoteCommandHandler : IRequestHandler<RemoveNoteCommand>
{
    private readonly RemoveNoteCommandHandler _inner;

    public CachedRemoveNoteCommandHandler(RemoveNoteCommandHandler inner)
    {
        _inner = inner;
    }

    public async Task Handle(RemoveNoteCommand request, CancellationToken cancellationToken)
    {
        await CacheInvalidator.GetInstance().AddCacheCts.CancelAsync();
        await _inner.Handle(request, cancellationToken);
    }
}
