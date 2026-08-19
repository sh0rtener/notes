using AutoMapper;
using MediatR;

namespace ShNotes.UseCases.Notes.GetNote;

public sealed class GetNoteQuery : IRequest<NoteDto>
{
    public int Id { get; set; }
}

public sealed class GetNoteQueryHandler : IRequestHandler<GetNoteQuery, NoteDto>
{
    private readonly IMapper _mapper;
    private readonly Core.Notes.INoteRepository _noteRepository;

    public GetNoteQueryHandler(IMapper mapper, Core.Notes.INoteRepository noteRepository)
    {
        _mapper = mapper;
        _noteRepository = noteRepository;
    }

    public async Task<NoteDto> Handle(GetNoteQuery request, CancellationToken cancellationToken)
    {
        var note = await _noteRepository.Get(request.Id, cancellationToken);
        return _mapper.Map<NoteDto>(note);
    }
}
