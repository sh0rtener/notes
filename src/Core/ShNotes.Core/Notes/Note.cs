using System.Text.RegularExpressions;

namespace ShNotes.Core.Notes;

public sealed class Note : Entity<int>
{
    private const string _nameRegex = @"^(?=.{4,24}$)[^*\\/'""^<>:|?]+$";
    public string Name { get; private set; }
    public string Description { get; private set; }
    public NoteStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Note(string name, string description, NoteStatus status)
    {
        if (string.IsNullOrEmpty(name) || !Regex.IsMatch(name, _nameRegex))
            throw new InvalidNoteNameException();

        if (description is not null && description.Length > 1000)
            throw new TooBigNoteDescriptionException();

        Name = name;
        Description = description ?? "";
        Status = status;

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeName(string value)
    {
        if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value, _nameRegex))
            throw new InvalidNoteNameException();

        Name = value;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeDescription(string value)
    {
        if (value is null)
            throw new CannotSetNullDescriptionException();

        if (value.Length > 1000)
            throw new TooBigNoteDescriptionException();

        Description = value;
        UpdatedAt = DateTime.UtcNow;
    }

    public void TakeToWork()
    {
        Status = NoteStatus.OnWork;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        Status = NoteStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }
}
