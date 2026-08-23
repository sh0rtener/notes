using MediatR;

namespace ShNotes.UseCases.Notes.RemoveNote;

public sealed class RemoveNoteCommand : IRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
}

public sealed class RemoveNoteCommandHandler : IRequestHandler<RemoveNoteCommand>
{
    private readonly Core.Notes.INoteRepository _repository;
    private readonly INoteRepository _noteRepositoryUseCase;

    public RemoveNoteCommandHandler(
        Core.Notes.INoteRepository repository,
        INoteRepository noteRepositoryUseCase
    )
    {
        _repository = repository;
        _noteRepositoryUseCase = noteRepositoryUseCase;
    }

    public async Task Handle(RemoveNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await _repository.Get(request.Id, cancellationToken);

        if (note is null)
            throw new NoteWasntFoundException();
        if (!await _noteRepositoryUseCase.IsUserNote(request.UserId, request.Id, cancellationToken))
            throw new NotePermissionException();

        await _repository.Remove(request.Id, cancellationToken);
    }
}
