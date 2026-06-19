using Microsoft.AspNetCore.Mvc;
using LiveEvents.Api.Common.Controllers;
using LiveEvents.Api.Events.Application.UseCases.Events.Dtos;
using LiveEvents.Api.Events.Application.UseCases.Events.Handlers;

namespace LiveEvents.Api.Events.Controllers;

[Route("Api/[controller]")]
public class EventsController(
    IEventCommandHandler eventCommandHandler,
    IEventQueryHandler eventQueryHandler) : ApiControllerBase
{
    private readonly IEventCommandHandler _eventCommandHandler = eventCommandHandler;
    private readonly IEventQueryHandler _eventQueryHandler = eventQueryHandler;

    [HttpGet("dropdown")]
    [ProducesResponseType(typeof(IEnumerable<EventDropdownDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetEventsForDropdown(CancellationToken cancellationToken)
    {
        var result = await _eventQueryHandler.GetEventsForDropdown(cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateEventDto request,
        CancellationToken cancellationToken)
    {
        var result = await _eventCommandHandler.CreateEvent(request, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : HandleError(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] EventFilterDto filter, CancellationToken cancellationToken)
    {
        var result = await _eventQueryHandler.GetAllEventsFiltered(filter, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _eventQueryHandler.GetEventById(id, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}/occupation-report")]
    public async Task<IActionResult> GetOccupationReport(Guid id, CancellationToken cancellationToken)
    {
        var result = await _eventQueryHandler.GetOccupationReport(id, cancellationToken);
        return HandleResult(result);
    }
}
