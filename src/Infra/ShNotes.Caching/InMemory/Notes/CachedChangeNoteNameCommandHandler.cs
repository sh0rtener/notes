using MediatR;
using Microsoft.Extensions.Caching.Memory;
using ShNotes.UseCases.Notes;
using ShNotes.UseCases.Notes.ChangeNoteName;

namespace ShNotes.Caching.InMemory.Notes;

public sealed class CachedChangeNoteNameCommandHandler
    : IRequestHandler<ChangeNoteNameCommand, NoteDto>
{
    private readonly ChangeNoteNameCommandHandler _inner;
    private readonly INoteRepository _noteRepositoryUseCase;

    public CachedChangeNoteNameCommandHandler(
        ChangeNoteNameCommandHandler inner,
        INoteRepository noteRepositoryUseCase
    )
    {
        _inner = inner;
        _noteRepositoryUseCase = noteRepositoryUseCase;
    }

    public async Task<NoteDto> Handle(
        ChangeNoteNameCommand request,
        CancellationToken cancellationToken
    )
    {
        if (!await _noteRepositoryUseCase.IsUserNote(request.UserId, request.Id, cancellationToken))
            throw new NotePermissionException();
        await CacheInvalidator.GetInstance().AddCacheCts.CancelAsync();
        return await _inner.Handle(request, cancellationToken);
    }
}
