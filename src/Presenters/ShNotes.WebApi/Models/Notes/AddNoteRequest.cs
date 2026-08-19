namespace ShNotes.WebApi.Models.Notes;

public sealed class AddNoteRequest
{
    public required string Name { get; set; }
    public string Description { get; set; } = "";
}
