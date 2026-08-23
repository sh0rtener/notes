using MediatR;
using Microsoft.Extensions.Caching.Memory;
using ShNotes.UseCases.Notes;
using ShNotes.UseCases.Notes.ChangeNoteStatus;

namespace ShNotes.Caching.InMemory.Notes;

public sealed class CachedChangeNoteStatusCommandHandler : IRequestHandler<ChangeNoteStatusCommand>
{
    private readonly ChangeNoteStatusCommandHandler _inner;
    private readonly INoteRepository _noteRepositoryUseCase;

    public CachedChangeNoteStatusCommandHandler(
        ChangeNoteStatusCommandHandler inner,
        INoteRepository noteRepositoryUseCase
    )
    {
        _inner = inner;
        _noteRepositoryUseCase = noteRepositoryUseCase;
    }

    public async Task Handle(ChangeNoteStatusCommand request, CancellationToken cancellationToken)
    {
        if (!await _noteRepositoryUseCase.IsUserNote(request.UserId, request.Id, cancellationToken))
            throw new NotePermissionException();
        await CacheInvalidator.GetInstance().AddCacheCts.CancelAsync();
        await _inner.Handle(request, cancellationToken);
    }
}
