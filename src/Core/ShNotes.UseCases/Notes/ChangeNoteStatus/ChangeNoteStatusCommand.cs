using MediatR;

namespace ShNotes.UseCases.Notes.ChangeNoteStatus;

public sealed class ChangeNoteStatusCommand : IRequest
{
    public int Id { get; set; }
    public NoteStatusEnum Status { get; set; }
}

public sealed class ChangeNoteStatusCommandHandler : IRequestHandler<ChangeNoteStatusCommand>
{
    private readonly Core.Notes.INoteRepository _noteRepository;

    public ChangeNoteStatusCommandHandler(Core.Notes.INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task Handle(ChangeNoteStatusCommand request, CancellationToken cancellationToken)
    {
        var note = await _noteRepository.Get(request.Id, cancellationToken);

        if (note is null)
            throw new NoteWasntFoundException();

        switch (request.Status)
        {
            case NoteStatusEnum.OnWork:
                note.TakeToWork();
                break;
            case NoteStatusEnum.Completed:
                note.Complete();
                break;
        }

        await _noteRepository.Update(request.Id, note, cancellationToken);
    }
}
