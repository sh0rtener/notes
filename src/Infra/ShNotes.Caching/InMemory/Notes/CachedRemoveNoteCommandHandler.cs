using MediatR;
using ShNotes.UseCases.Notes;
using ShNotes.UseCases.Notes.RemoveNote;

namespace ShNotes.Caching.InMemory.Notes;

public sealed class CachedRemoveNoteCommandHandler : IRequestHandler<RemoveNoteCommand>
{
    private readonly RemoveNoteCommandHandler _inner;
    private readonly INoteRepository _noteRepositoryUseCase;

    public CachedRemoveNoteCommandHandler(
        RemoveNoteCommandHandler inner,
        INoteRepository noteRepositoryUseCase
    )
    {
        _inner = inner;
        _noteRepositoryUseCase = noteRepositoryUseCase;
    }

    public async Task Handle(RemoveNoteCommand request, CancellationToken cancellationToken)
    {
        if (!await _noteRepositoryUseCase.IsUserNote(request.UserId, request.Id, cancellationToken))
            throw new NotePermissionException();
        await CacheInvalidator.GetInstance().AddCacheCts.CancelAsync();
        await _inner.Handle(request, cancellationToken);
    }
}
