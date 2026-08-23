using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShNotes.Core.Notes;

namespace ShNotes.Data.EntityFramework;

public sealed class NoteRepository : INoteRepository
{
    private readonly AppDbContext _context;
    private readonly IScriptProvider _scriptProvider;
    private readonly IMapper _mapper;

    public NoteRepository(AppDbContext context, IScriptProvider scriptProvider, IMapper mapper)
    {
        _context = context;
        _scriptProvider = scriptProvider;
        _mapper = mapper;
    }

    public async Task<int> Add(int userId, Note note, CancellationToken cancellationToken = default)
    {
        var parameters = _scriptProvider.AddNoteParameters(note, userId);
        var noteId = (
            await _context
                .Database.SqlQueryRaw<int>(_scriptProvider.AddNote, parameters.ToArray())
                .ToListAsync(cancellationToken)
        ).First();

        return noteId;
    }

    public async Task<Note?> Get(int noteId, CancellationToken cancellationToken = default)
    {
        var parameters = _scriptProvider.GetNoteParameter(noteId);
        var note = await _context
            .Notes.FromSqlRaw(_scriptProvider.GetNote, parameters.ToArray())
            .FirstOrDefaultAsync(cancellationToken);

        return note is null ? null : _mapper.Map<Note>(note);
    }

    public async Task Remove(int id, CancellationToken cancellationToken = default)
    {
        var parameters = _scriptProvider.DeleteNoteParameters(id);
        await _context.Database.ExecuteSqlRawAsync(
            _scriptProvider.DeleteNote,
            parameters.ToArray(),
            cancellationToken
        );
    }

    public async Task<Note> Update(int id, Note note, CancellationToken cancellationToken = default)
    {
        var parameters = _scriptProvider.UpdateNoteParameters(id, note);
        await _context.Database.ExecuteSqlRawAsync(
            _scriptProvider.UpdateNote,
            parameters.ToArray(),
            cancellationToken
        );

        return note;
    }
}
