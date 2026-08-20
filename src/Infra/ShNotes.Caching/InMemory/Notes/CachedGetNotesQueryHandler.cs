using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using ShNotes.UseCases.Notes;
using ShNotes.UseCases.Notes.GetNotes;

namespace ShNotes.Caching.InMemory.Notes;

public sealed class CachedGetNotesQueryHandler
    : IRequestHandler<GetNotesQuery, IEnumerable<ShortNoteDto>>
{
    private readonly GetNotesQueryHandler _inner;
    private readonly IMemoryCache _memoryCache;

    public CachedGetNotesQueryHandler(GetNotesQueryHandler inner, IMemoryCache memoryCache)
    {
        _inner = inner;
        _memoryCache = memoryCache;
    }

    public async Task<IEnumerable<ShortNoteDto>> Handle(
        GetNotesQuery request,
        CancellationToken cancellationToken
    )
    {
        var key = "notes_" + request.GetHashCode();

        _memoryCache.TryGetValue(key, out var notes);

        if (notes is not null)
            return (IEnumerable<ShortNoteDto>)notes;

        var result = await _inner.Handle(request, cancellationToken);
        _memoryCache.Set(
            key,
            result,
            new MemoryCacheEntryOptions()
                .AddExpirationToken(
                    new CancellationChangeToken(CacheInvalidator.GetInstance().AddCacheCts.Token)
                )
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
        );

        return result;
    }
}
