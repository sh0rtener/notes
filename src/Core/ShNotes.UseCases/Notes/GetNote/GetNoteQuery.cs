using AutoMapper;
using MediatR;

namespace ShNotes.UseCases.Notes.GetNote;

public sealed class GetNoteQuery : IRequest<NoteDto>
{
    public int Id { get; set; }
    public int UserId { get; set; }
}

public sealed class GetNoteQueryHandler : IRequestHandler<GetNoteQuery, NoteDto>
{
    private readonly IMapper _mapper;
    private readonly Core.Notes.INoteRepository _noteRepository;
    private readonly INoteRepository _noteRepositoryUseCase;

    public GetNoteQueryHandler(
        IMapper mapper,
        Core.Notes.INoteRepository noteRepository,
        INoteRepository noteRepositoryUseCase
    )
    {
        _mapper = mapper;
        _noteRepository = noteRepository;
        _noteRepositoryUseCase = noteRepositoryUseCase;
    }

    public async Task<NoteDto> Handle(GetNoteQuery request, CancellationToken cancellationToken)
    {
        var note = await _noteRepository.Get(request.Id, cancellationToken);
        if (!await _noteRepositoryUseCase.IsUserNote(request.UserId, request.Id, cancellationToken))
            throw new NotePermissionException();
        return _mapper.Map<NoteDto>(note);
    }
}
