using AutoMapper;
using MediatR;

namespace ShNotes.UseCases.Notes.ChangeNoteName;

public sealed class ChangeNoteDescriptionCommand : IRequest<NoteDto>
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string Description { get; set; }
}

public sealed class ChangeNoteDescriptionCommandHandler
    : IRequestHandler<ChangeNoteDescriptionCommand, NoteDto>
{
    private readonly Core.Notes.INoteRepository _noteRepository;
    private readonly INoteRepository _noteRepositoryUseCase;
    private readonly IMapper _mapper;

    public ChangeNoteDescriptionCommandHandler(
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
        ChangeNoteDescriptionCommand request,
        CancellationToken cancellationToken
    )
    {
        var note = await _noteRepository.Get(request.Id, cancellationToken);

        if (note is null)
            throw new NoteWasntFoundException();

        if (!await _noteRepositoryUseCase.IsUserNote(request.UserId, request.Id, cancellationToken))
            throw new NotePermissionException();

        if (note.Name.Equals(request.Description))
            return _mapper.Map<NoteDto>(note);

        note.ChangeName(request.Description);

        await _noteRepository.Update(request.Id, note, cancellationToken);

        return _mapper.Map<NoteDto>(note);
    }
}
