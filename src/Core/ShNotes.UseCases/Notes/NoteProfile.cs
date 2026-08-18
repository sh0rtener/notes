using AutoMapper;
using ShNotes.Core.Notes;

namespace ShNotes.UseCases.Notes;

public sealed class NoteProfile : Profile
{
    public NoteProfile()
    {
        CreateMap<Note, NoteProfile>().ReverseMap();
    }
}
