namespace ShNotes.UseCases.Notes;

public sealed class ShortNoteDto
{
    /// <summary>
    /// Идентификатор заметки
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }  
    /// <summary>
    /// Наименование заметки
    /// </summary>
    /// <example>Создать данное приложение</example>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Статус заметки
    /// </summary>
    /// <example>new</example>
    public string Status { get; set; } = null!;
    /// <summary>
    /// Дата обновления заметки
    /// </summary>
    /// <example>2026-01-01</example>
    public DateTime UpdatedAt { get; set; }
}
