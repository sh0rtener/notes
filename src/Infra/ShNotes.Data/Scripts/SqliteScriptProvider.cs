using System.Data.Common;
using Microsoft.Data.Sqlite;
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

    public List<DbParameter> GetNotesParameters(GetNoteFilter filter)
    {
        return
        [
            new SqliteParameter("@name", filter.Name ?? (object)DBNull.Value),
            new SqliteParameter("@status", filter.Status ?? (object)DBNull.Value),
            new SqliteParameter("@date", filter.DateFrom ?? (object)DBNull.Value),
            new SqliteParameter("@limit", filter.Limit),
            new SqliteParameter("@offset", filter.Offset),
        ];
    }
}
