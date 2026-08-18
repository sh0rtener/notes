namespace ShNotes.UseCases.Notes;

public sealed class ShortNoteDto
{
    public int Id { get; set; }  
    public string Name { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime UpdatedAt { get; set; }
}
