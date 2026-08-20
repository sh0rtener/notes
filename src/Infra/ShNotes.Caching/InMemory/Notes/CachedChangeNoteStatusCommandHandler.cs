using MediatR;
using Microsoft.Extensions.Caching.Memory;
using ShNotes.UseCases.Notes.ChangeNoteStatus;

namespace ShNotes.Caching.InMemory.Notes;

public sealed class CachedChangeNoteStatusCommandHandler : IRequestHandler<ChangeNoteStatusCommand>
{
    private readonly ChangeNoteStatusCommandHandler _inner;

    public CachedChangeNoteStatusCommandHandler(
        ChangeNoteStatusCommandHandler inner
    )
    {
        _inner = inner;
    }

    public async Task Handle(ChangeNoteStatusCommand request, CancellationToken cancellationToken)
    {
        await CacheInvalidator.GetInstance().AddCacheCts.CancelAsync();
        await _inner.Handle(request, cancellationToken);
    }
}
