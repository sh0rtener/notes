using AutoMapper;
using ShNotes.Core.Notes;

namespace ShNotes.UseCases.Notes;

public sealed class NoteDtoProfile : Profile
{
    public NoteDtoProfile()
    {
        CreateMap<NoteDto, Note>()
            .ForMember(x => x.Status, u => u.MapFrom(x => new NoteStatus(x.Status)));

        CreateMap<Note, NoteDto>().ForMember(x => x.Status, u => u.MapFrom(x => x.Status.Name));
    }
}

public sealed class NoteDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
