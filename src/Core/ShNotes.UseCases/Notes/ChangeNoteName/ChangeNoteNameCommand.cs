using AutoMapper;
using MediatR;

namespace ShNotes.UseCases.Notes.ChangeNoteName;

public sealed class ChangeNoteNameCommand : IRequest<NoteDto>
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string Name { get; set; }
}

public sealed class ChangeNoteNameCommandHandler : IRequestHandler<ChangeNoteNameCommand, NoteDto>
{
    private readonly Core.Notes.INoteRepository _noteRepository;
    private readonly INoteRepository _noteRepositoryUseCase;
    private readonly IMapper _mapper;

    public ChangeNoteNameCommandHandler(
        Core.Notes.INoteRepository noteRepository,
        IMapper mapper,
        INoteRepository noteRepositoryUseCase
    )
    {
        _noteRepository = noteRepository;
        _mapper = mapper;
        _noteRepositoryUseCase = noteRepositoryUseCase;
    }

    public async Task<NoteDto> Handle(
        ChangeNoteNameCommand request,
        CancellationToken cancellationToken
    )
    {
        var note = await _noteRepository.Get(request.Id, cancellationToken);

        if (note is null)
            throw new NoteWasntFoundException();
        if (!await _noteRepositoryUseCase.IsUserNote(request.UserId, request.Id, cancellationToken))
            throw new NotePermissionException();

        if (note.Name.Equals(request.Name))
            return _mapper.Map<NoteDto>(note);

        note.ChangeName(request.Name);

        await _noteRepository.Update(request.Id, note, cancellationToken);

        return _mapper.Map<NoteDto>(note);
    }
}
