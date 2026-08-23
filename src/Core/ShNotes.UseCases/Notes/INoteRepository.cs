using System.Text.Json.Serialization;

namespace ShNotes.UseCases.Notes;

public sealed class GetNoteFilter
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    [JsonIgnore]
    public int UserId { get; set; }

    /// <summary>
    /// Наименование заметки
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Статус заявки, может принимать значения:
    /// (new - новая заметка; onwork - заявка в работе; completed - готовые)
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Дата обновления заметки
    /// </summary>
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// Лимит данных
    /// </summary>
    public int Limit { get; set; } = 100;

    /// <summary>
    /// Отступ от первичного значения
    /// </summary>
    public int Offset { get; set; } = 0;
}

public interface INoteRepository
{
    Task<IEnumerable<ShortNoteDto>> Get(
        GetNoteFilter filter,
        CancellationToken cancellationToken = default
    );

    Task<bool> IsUserNote(int userId, int noteId, CancellationToken cancellationToken = default);
}
