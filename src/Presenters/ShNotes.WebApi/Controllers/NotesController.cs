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
using ShNotes.WebApi.Swagger;

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

    /// <summary>
    /// Получение всех заметок
    /// </summary>
    /// <param name="getNoteFilter">Фильтр поиска</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Список заметок относительно заданных фильтров</returns>
    /// <response code="200">Успешно!</response>
    /// <response code="400">Пользовательская ошибка</response>
    [HttpGet("all")]
    [ProducesResponseType(
        typeof(SuccessApiResponse<IEnumerable<ShortNoteDto>>),
        StatusCodes.Status200OK
    )]
    [ProducesResponseType(typeof(BadRequestApiResponse), StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Получение конкретной заметки
    /// </summary>
    /// <param name="id">Идентификатор заметки</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Заметка</returns>
    /// <response code="200">Успешно!</response>
    /// <response code="400">Пользовательская ошибка</response>
    [HttpGet]
    [ProducesResponseType(typeof(SuccessApiResponse<NoteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get([FromQuery] int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetNoteQuery() { Id = id }, cancellationToken);
        return this.SendOkResult(result);
    }

    /// <summary>
    /// Добавление заявки
    /// </summary>
    /// <param name="request">Модель создания заявки</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Идентификатор добавленной заявки</returns>
    /// <response code="200">Успешно!</response>
    /// <response code="400">Пользовательская ошибка</response>
    [HttpPost]
    [ProducesResponseType(typeof(SuccessApiResponse<int>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestApiResponse), StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Изменение имени заметки
    /// </summary>
    /// <param name="id">Идентификатор заметки</param>
    /// <param name="request">Модель с новым наименованием</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Обновленная модель</returns>
    /// <response code="200">Успешно!</response>
    /// <response code="400">Пользовательская ошибка</response>
    [HttpPatch("{id}/name")]
    [ProducesResponseType(typeof(SuccessApiResponse<NoteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestApiResponse), StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Изменение описания заметки
    /// </summary>
    /// <param name="id">Идентификатор заметки</param>
    /// <param name="request">Модель с новым описанием</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Обновленная модель</returns>
    /// <response code="200">Успешно!</response>
    /// <response code="400">Пользовательская ошибка</response>
    [HttpPatch("{id}/description")]
    [ProducesResponseType(typeof(SuccessApiResponse<NoteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestApiResponse), StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Задать заметке статус "В работе"
    /// </summary>
    /// <param name="id">Идентификатор заметки</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Статус заметки</returns>
    /// <response code="200">Успешно!</response>
    /// <response code="400">Пользовательская ошибка</response>
    [HttpPatch("{id}/to-work")]
    [ProducesResponseType(typeof(SuccessApiResponse<NoteStatusEnum>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> TakeToWork(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new ChangeNoteStatusCommand() { Id = id, Status = NoteStatusEnum.OnWork },
            cancellationToken
        );
        return this.SendOkResult(NoteStatusEnum.OnWork);
    }

    /// <summary>
    /// Задать заметке статус "Выполнено"
    /// </summary>
    /// <param name="id">Идентификатор заметки</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Статус заметки</returns>
    /// <response code="200">Успешно!</response>
    /// <response code="400">Пользовательская ошибка</response>
    [HttpPatch("{id}/complete")]
    [ProducesResponseType(typeof(SuccessApiResponse<NoteStatusEnum>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Complete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new ChangeNoteStatusCommand() { Id = id, Status = NoteStatusEnum.Completed },
            cancellationToken
        );
        return this.SendOkResult(NoteStatusEnum.Completed);
    }

    /// <summary>
    /// Удаление заметки
    /// </summary>
    /// <param name="id">Идентификатор заметки</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Идентификатор удаленной заметки</returns>
    /// <response code="200">Успешно!</response>
    /// <response code="400">Пользовательская ошибка</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(SuccessApiResponse<int>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemoveNoteCommand() { Id = id }, cancellationToken);
        return this.SendOkResult(id);
    }
}
