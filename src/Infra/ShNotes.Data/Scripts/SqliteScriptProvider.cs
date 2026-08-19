using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Data.Sqlite;
using ShNotes.Core.Notes;
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
                notes (name, description, status, created_at, updated_at)
            VALUES 
                (@name, @description, @status, @created_at, @updated_at)
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

    public List<DbParameter> AddNoteParameters(Note note) =>
        [
            new SqliteParameter("@name", note.Name),
            new SqliteParameter("@description", note.Description),
            new SqliteParameter("@status", note.Status.Name),
            new SqliteParameter("@created_at", note.CreatedAt),
            new SqliteParameter("@updated_at", note.UpdatedAt),
        ];

    public List<DbParameter> DeleteNoteParameters(int id) => [new SqliteParameter("@id", id)];

    public List<DbParameter> GetNoteParameter(int id) => [new SqliteParameter("@id", id)];

    public List<DbParameter> GetNotesParameters(GetNoteFilter filter) =>
        [
            new SqliteParameter("@name", filter.Name ?? (object)DBNull.Value),
            new SqliteParameter("@status", filter.Status ?? (object)DBNull.Value),
            new SqliteParameter("@date", filter.DateFrom ?? (object)DBNull.Value),
            new SqliteParameter("@limit", filter.Limit),
            new SqliteParameter("@offset", filter.Offset),
        ];

    public List<DbParameter> UpdateNoteParameters(int id, Note note) =>
        [
            new SqliteParameter("@name", note.Name),
            new SqliteParameter("@description", note.Description),
            new SqliteParameter("@status", note.Status.Name),
            new SqliteParameter("@updated_at", note.UpdatedAt),
            new SqliteParameter("@id", id),
        ];
}
