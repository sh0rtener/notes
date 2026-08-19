namespace ShNotes.Core.Notes;

public sealed class NoteStatus : ValueObject
{
    private string _name;
    public string Name => _name;

    public static NoteStatus New => new("new");
    public static NoteStatus OnWork => new("onwork");
    public static NoteStatus Completed => new("completed");

    public NoteStatus(string name) => _name = name;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
