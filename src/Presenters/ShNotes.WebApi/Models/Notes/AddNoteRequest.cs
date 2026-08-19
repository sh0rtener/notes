namespace ShNotes.WebApi.Models.Notes;

public sealed class AddNoteRequest
{
    /// <summary>Наименование заметки</summary>
    /// <example>Task #1</example>
    public required string Name { get; set; }

    /// <summary>Описание заметки</summary>
    /// <example>First of all we need to did this actions: ...</example>
    public string Description { get; set; } = "";
}
