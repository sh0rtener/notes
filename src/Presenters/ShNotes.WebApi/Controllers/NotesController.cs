using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShNotes.UseCases.Notes;
using ShNotes.UseCases.Notes.GetNotes;
using ShNotes.WebApi.Common;

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

    [HttpGet]
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
}
