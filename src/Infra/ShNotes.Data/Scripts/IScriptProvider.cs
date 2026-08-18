using System.Data.Common;
using ShNotes.UseCases.Notes;

namespace ShNotes.Data.Scripts;

public interface IScriptProvider
{
    public string GetNotes { get; }
    public List<DbParameter> GetNotesParameters(GetNoteFilter filter);
}
