using System.Data.Common;
using ShNotes.Core.Notes;
using ShNotes.UseCases.Notes;

namespace ShNotes.Data.Scripts;

public interface IScriptProvider
{
    public string GetNotes { get; }
    public List<DbParameter> GetNotesParameters(GetNoteFilter filter);

    public string GetNote { get; }
    public List<DbParameter> GetNoteParameter(int id);

    public string AddNote { get; }
    public List<DbParameter> AddNoteParameters(Note note);

    public string UpdateNote { get; }
    public List<DbParameter> UpdateNoteParameters(int id, Note note);

    public string DeleteNote { get; }
    public List<DbParameter> DeleteNoteParameters(int id);
}
