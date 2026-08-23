using System.Data.Common;
using System.Diagnostics;
using ShNotes.Core.Notes;
using ShNotes.Core.Users;
using ShNotes.UseCases.Notes;

namespace ShNotes.Data.Scripts;

public interface IScriptProvider
{
    public string GetNotes { get; }
    public List<DbParameter> GetNotesParameters(GetNoteFilter filter);

    public string GetNote { get; }
    public List<DbParameter> GetNoteParameter(int id);
    public string AddNote { get; }
    public List<DbParameter> AddNoteParameters(Note note, int userId);
    public string UpdateNote { get; }
    public List<DbParameter> UpdateNoteParameters(int id, Note note);
    public string DeleteNote { get; }
    public List<DbParameter> DeleteNoteParameters(int id);
    public string CreateUser { get; }
    public List<DbParameter> CreateUserParameters(User user);
    public string IsExistsUser { get; }
    public List<DbParameter> IsExistsUserParameters(string name);
    public string GetUser { get; }
    public List<DbParameter> GetUserParameters(int id);
    public string GetUserByUsername { get; }
    public List<DbParameter> GetUserByUsernameParameters(string username);
    public string IsUserNote { get; }
    public List<DbParameter> IsUserNoteParameters(int userId, int noteId);
}
