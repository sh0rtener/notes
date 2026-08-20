using MediatR;
using Microsoft.Extensions.Caching.Memory;
using ShNotes.UseCases.Notes;
using ShNotes.UseCases.Notes.ChangeNoteName;

namespace ShNotes.Caching.InMemory.Notes;

public sealed class CachedChangeNoteNameCommandHandler
    : IRequestHandler<ChangeNoteNameCommand, NoteDto>
{
    private readonly ChangeNoteNameCommandHandler _inner;

    public CachedChangeNoteNameCommandHandler(
        ChangeNoteNameCommandHandler inner
    )
    {
        _inner = inner;
    }

    public async Task<NoteDto> Handle(
        ChangeNoteNameCommand request,
        CancellationToken cancellationToken
    )
    {
        await CacheInvalidator.GetInstance().AddCacheCts.CancelAsync();
        return await _inner.Handle(request, cancellationToken);
    }
}
