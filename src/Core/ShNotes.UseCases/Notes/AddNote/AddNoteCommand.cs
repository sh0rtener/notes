using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ShNotes.Core.Notes;

namespace ShNotes.UseCases.Notes.AddNote;

public sealed class AddNoteCommand : IRequest<int>
{
    public required string Name { get; set; }
    public string Description { get; set; } = null!;
}

public sealed class AddNoteCommandHandler : IRequestHandler<AddNoteCommand, int>
{
    private readonly Core.Notes.INoteRepository _noteRepository;

    public AddNoteCommandHandler(Core.Notes.INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<int> Handle(AddNoteCommand request, CancellationToken cancellationToken)
    {
        var note = new Note(request.Name, request.Description, NoteStatus.New);
        var addedId = await _noteRepository.Add(note, cancellationToken);

        return addedId;
    }
}
