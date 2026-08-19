using MediatR;

namespace ShNotes.UseCases.Notes.RemoveNote;

public sealed class RemoveNoteCommand : IRequest
{
    public int Id { get; set; }
}

public sealed class RemoveNoteCommandHandler : IRequestHandler<RemoveNoteCommand>
{
    private readonly Core.Notes.INoteRepository _repository;

    public RemoveNoteCommandHandler(Core.Notes.INoteRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(RemoveNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await _repository.Get(request.Id, cancellationToken);

        if (note is null)
            throw new NoteWasntFoundException();

        await _repository.Remove(request.Id, cancellationToken);
    }
}
