using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShNotes.UseCases.Notes;
using ShNotes.UseCases.Notes.AddNote;
using ShNotes.UseCases.Notes.ChangeNoteName;
using ShNotes.UseCases.Notes.ChangeNoteStatus;
using ShNotes.UseCases.Notes.GetNote;
using ShNotes.UseCases.Notes.GetNotes;
using ShNotes.UseCases.Notes.RemoveNote;
using ShNotes.WebApi.Common;
using ShNotes.WebApi.Models.Notes;

namespace ShNotes.WebApi.Controllers;

[ApiController]
[Route("notes")]
public sealed class NotesController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("all")]
    public async Task<IActionResult> Get(
        [FromQuery] GetNoteFilter getNoteFilter,
        CancellationToken cancellationToken
    )
    {
        var result = await _mediator.Send(
            new GetNotesQuery() { Filter = getNoteFilter },
            cancellationToken
        );

        return this.SendOkResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetNoteQuery() { Id = id }, cancellationToken);
        return this.SendOkResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add(
        [FromBody] AddNoteRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await _mediator.Send(
            new AddNoteCommand() { Name = request.Name, Description = request.Description },
            cancellationToken
        );

        return this.SendOkResult(result);
    }

    [HttpPatch("{id}/name")]
    public async Task<IActionResult> ChangeNoteName(
        int id,
        [FromBody] ChangeNoteNameRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await _mediator.Send(
            new ChangeNoteNameCommand() { Id = id, Name = request.Name },
            cancellationToken
        );
        return this.SendOkResult(result);
    }

    [HttpPatch("{id}/description")]
    public async Task<IActionResult> ChangeNodeDescription(
        int id,
        [FromBody] ChangeNoteDescriptionRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await _mediator.Send(
            new ChangeNoteDescriptionCommand() { Id = id, Description = request.Description },
            cancellationToken
        );
        return this.SendOkResult(result);
    }

    [HttpPatch("{id}/to-work")]
    public async Task<IActionResult> TakeToWork(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new ChangeNoteStatusCommand() { Id = id, Status = NoteStatusEnum.OnWork },
            cancellationToken
        );
        return this.SendOkResult(NoteStatusEnum.OnWork);
    }

    [HttpPatch("{id}/complete")]
    public async Task<IActionResult> Complete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new ChangeNoteStatusCommand() { Id = id, Status = NoteStatusEnum.Completed },
            cancellationToken
        );
        return this.SendOkResult(NoteStatusEnum.Completed);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemoveNoteCommand() { Id = id }, cancellationToken);
        return this.SendOkResult(id);
    }
}
