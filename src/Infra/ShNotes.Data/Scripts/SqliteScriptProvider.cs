using System.Data.Common;
using Microsoft.Data.Sqlite;
using ShNotes.Core.Notes;
using ShNotes.Core.Users;
using ShNotes.UseCases.Notes;

namespace ShNotes.Data.Scripts;

public sealed class SqliteScriptProvider : IScriptProvider
{
    public string GetNotes =>
        """
                SELECT 
                    n.id,
                    n.name,
                    n.description,
                    n.status,
                    n.created_at,
                    n.updated_at
                FROM
                    notes n
                WHERE
                    (@name IS NULL OR n.name LIKE '%' || @name || '%')
                    AND (@status IS NULL OR n.status LIKE '%' || @status || '%')
                    AND (@date IS NULL OR n.created_at >= @date)
                    AND n.user_id = @user_id
                LIMIT 
                    @limit
                OFFSET
                    @offset
            """;

    public string GetNote =>
        """
            SELECT 
                n.id,
                n.name,
                n.description,
                n.status,
                n.created_at,
                n.updated_at
            FROM
                notes n
            WHERE
                n.id = @id
            """;

    public string AddNote =>
        """
            INSERT INTO 
                notes (name, description, status, created_at, updated_at, user_id)
            VALUES 
                (@name, @description, @status, @created_at, @updated_at, @user_id)
            RETURNING id;
            """;

    public string UpdateNote =>
        """
                UPDATE
                    notes
                SET
                    name = @name,
                    description = @description,
                    status = @status,
                    updated_at = @updated_at
                WHERE
                    id = @id
            """;

    public string DeleteNote =>
        """
                DELETE FROM
                    notes
                WHERE
                    id = @id
            """;

    public string CreateUser =>
        """
                INSERT INTO 
                    users (name, created_at)
                VALUES
                    (@name, @created_at);
                    
                INSERT INTO 
                    credentials (password_hash, password_salt, created_at, user_id)
                VALUES 
                    (@password_hash, @password_salt, @created_at, last_insert_rowid())
                RETURNING user_id;                
            """;

    public string IsExistsUser =>
        """
            SELECT 
                1
            FROM
                users
            WHERE
                name = @name
            """;

    public string GetUser =>
        """
                SELECT 
                    u.id,
                    u.name,
                    u.created_at,
                    c.id as credential_id,
                    c.password_hash,
                    c.password_salt,
                    c.created_at as c_created_at
                FROM
                    users u
                JOIN
                    credentials c ON c.user_id = u.id
                WHERE
                    u.id = @id
            """;

    public string GetUserByUsername =>
        """
                SELECT 
                    u.id,
                    u.name,
                    u.created_at,
                    c.id as credential_id,
                    c.password_hash,
                    c.password_salt,
                    c.created_at as c_created_at
                FROM
                    users u
                JOIN
                    credentials c ON c.user_id = u.id
                WHERE
                    u.name = @name
            """;

    public string IsUserNote =>
        """
                SELECT 
                    1
                FROM
                    notes n
                WHERE
                    n.id = @note_id AND n.user_id = @user_id
            """;

    public List<DbParameter> AddNoteParameters(Note note, int userId) =>
        [
            new SqliteParameter("@user_id", userId),
            new SqliteParameter("@name", note.Name),
            new SqliteParameter("@description", note.Description),
            new SqliteParameter("@status", note.Status.Name),
            new SqliteParameter("@created_at", note.CreatedAt),
            new SqliteParameter("@updated_at", note.UpdatedAt),
        ];

    public List<DbParameter> CreateUserParameters(User user) =>
        [
            new SqliteParameter("@name", user.Name),
            new SqliteParameter("@created_at", user.CreatedAt),
            new SqliteParameter("@password_hash", user.Credential.PasswordHash),
            new SqliteParameter("@password_salt", user.Credential.PasswordSalt),
        ];

    public List<DbParameter> DeleteNoteParameters(int id) => [new SqliteParameter("@id", id)];

    public List<DbParameter> GetNoteParameter(int id) => [new SqliteParameter("@id", id)];

    public List<DbParameter> GetNotesParameters(GetNoteFilter filter) =>
        [
            new SqliteParameter("@user_id", filter.UserId),
            new SqliteParameter("@name", filter.Name ?? (object)DBNull.Value),
            new SqliteParameter("@status", filter.Status ?? (object)DBNull.Value),
            new SqliteParameter("@date", filter.DateFrom ?? (object)DBNull.Value),
            new SqliteParameter("@limit", filter.Limit),
            new SqliteParameter("@offset", filter.Offset),
        ];

    public List<DbParameter> GetUserByUsernameParameters(string username) =>
        [new SqliteParameter("@name", username)];

    public List<DbParameter> GetUserParameters(int id) => [new SqliteParameter("@id", id)];

    public List<DbParameter> IsExistsUserParameters(string name) =>
        [new SqliteParameter("@name", name)];

    public List<DbParameter> IsUserNoteParameters(int userId, int noteId) =>
        [new SqliteParameter("@note_id", noteId), new SqliteParameter("@user_id", userId)];

    public List<DbParameter> UpdateNoteParameters(int id, Note note) =>
        [
            new SqliteParameter("@name", note.Name),
            new SqliteParameter("@description", note.Description),
            new SqliteParameter("@status", note.Status.Name),
            new SqliteParameter("@updated_at", note.UpdatedAt),
            new SqliteParameter("@id", id),
        ];
}
