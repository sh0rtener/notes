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
    /// <summary>Идентификатор заметки</summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>Наименование заметки</summary>
    /// <example>Create this app</example>
    public string Name { get; set; } = null!;

    /// <summary>Описание заметки</summary>
    /// <example>First of all we need to did this actions: ...</example>
    public string Description { get; set; } = null!;

    /// <summary>Статус заметки</summary>
    /// <example>new</example>
    public string Status { get; set; } = null!;

    /// <summary>Дата создания заметки</summary>
    /// <example>2026-01-01</example>
    public DateTime CreatedAt { get; set; }

    /// <summary>Дата изменения заметки</summary>
    /// <example>2026-02-01</example>
    public DateTime UpdatedAt { get; set; }
}
