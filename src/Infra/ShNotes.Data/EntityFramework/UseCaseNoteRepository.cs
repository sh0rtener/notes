using System.Data.Common;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShNotes.UseCases.Notes;

namespace ShNotes.Data.EntityFramework;

public sealed class UseCaseNoteRepository : INoteRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IScriptProvider _scriptProvider;

    public UseCaseNoteRepository(
        AppDbContext context,
        IScriptProvider scriptProvider,
        IMapper mapper
    )
    {
        _context = context;
        _scriptProvider = scriptProvider;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ShortNoteDto>> Get(
        GetNoteFilter filter,
        CancellationToken cancellationToken = default
    )
    {
        var parameters = _scriptProvider.GetNotesParameters(filter);

        var notes = await _context
            .Notes.FromSqlRaw(_scriptProvider.GetNotes, parameters.ToArray())
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<ShortNoteDto>>(notes);
    }

    public async Task<bool> IsUserNote(
        int userId,
        int noteId,
        CancellationToken cancellationToken = default
    )
    {
        var parameters = _scriptProvider.IsUserNoteParameters(userId, noteId);
        var result = (
            await _context
                .Database.SqlQueryRaw<int>(_scriptProvider.IsUserNote, parameters.ToArray())
                .ToListAsync(cancellationToken)
        );

        return result is not null && result.Count >= 1;
    }
}
