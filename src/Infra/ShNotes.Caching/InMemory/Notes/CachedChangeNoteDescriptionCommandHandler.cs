using MediatR;
using Microsoft.Extensions.Caching.Memory;
using ShNotes.UseCases.Notes;
using ShNotes.UseCases.Notes.ChangeNoteName;

namespace ShNotes.Caching.InMemory.Notes;

public sealed class CachedChangeNoteDescriptionCommandHandler
    : IRequestHandler<ChangeNoteDescriptionCommand, NoteDto>
{
    private readonly ChangeNoteDescriptionCommandHandler _inner;

    public CachedChangeNoteDescriptionCommandHandler(
        ChangeNoteDescriptionCommandHandler inner
    )
    {
        _inner = inner;
    }

    public async Task<NoteDto> Handle(
        ChangeNoteDescriptionCommand request,
        CancellationToken cancellationToken
    )
    {
        await CacheInvalidator.GetInstance().AddCacheCts.CancelAsync();
        return await _inner.Handle(request, cancellationToken);
    }
}
