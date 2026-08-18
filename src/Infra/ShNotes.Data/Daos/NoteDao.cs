using AutoMapper;
using ShNotes.Core.Notes;
using ShNotes.UseCases.Notes;

namespace ShNotes.Data.Daos;

public sealed class NoteDaoProfile : Profile
{
    public NoteDaoProfile()
    {
        CreateMap<NoteDao, Note>().ReverseMap();
        CreateMap<NoteDao, ShortNoteDto>().ReverseMap();
    }
}

public sealed class NoteDao
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
