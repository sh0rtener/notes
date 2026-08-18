using MediatR;

namespace ShNotes.UseCases.Notes.GetNotes;

public sealed class GetNotesQuery : IRequest<IEnumerable<ShortNoteDto>>
{
    public required GetNoteFilter Filter { get; set; }
}

public sealed class GetNotesQueryHandler : IRequestHandler<GetNotesQuery, IEnumerable<ShortNoteDto>>
{
    private readonly INoteRepository _noteRepository;

    public GetNotesQueryHandler(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<IEnumerable<ShortNoteDto>> Handle(
        GetNotesQuery request,
        CancellationToken cancellationToken
    )
    {
        var result = await _noteRepository.Get(request.Filter, cancellationToken);
        return result;
    }
}
