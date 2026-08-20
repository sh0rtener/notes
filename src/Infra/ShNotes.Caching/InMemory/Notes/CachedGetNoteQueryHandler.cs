using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using ShNotes.UseCases.Notes;
using ShNotes.UseCases.Notes.GetNote;

namespace ShNotes.Caching.InMemory.Notes;

public sealed class CachedGetNoteQueryHandler : IRequestHandler<GetNoteQuery, NoteDto>
{
    private readonly IMemoryCache _cache;
    private readonly GetNoteQueryHandler _handler;

    public CachedGetNoteQueryHandler(IMemoryCache cache, GetNoteQueryHandler inner)
    {
        _handler = inner;
        _cache = cache;
    }

    public async Task<NoteDto> Handle(GetNoteQuery request, CancellationToken cancellationToken)
    {
        _cache.TryGetValue(request.Id.ToString(), out var note);

        if (note is not null)
            return (NoteDto)note;

        var result = await _handler.Handle(request, cancellationToken);
        _cache.Set(
            "notes_" + request.Id.ToString(),
            result,
            new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
        );

        return result;
    }
}
