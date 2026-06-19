using Microsoft.AspNetCore.Mvc;
using LiveEvents.Api.Common.Controllers;
using LiveEvents.Api.Events.Application.UseCases.Venues.Dtos;
using LiveEvents.Api.Events.Application.UseCases.Venues.Handlers;

namespace LiveEvents.Api.Events.Controllers;

[Route("Api/[controller]")]
public class VenuesController(IVenueQueryHandler venueQueryHandler) : ApiControllerBase
{
    private readonly IVenueQueryHandler _venueQueryHandler = venueQueryHandler;

    [HttpGet("dropdown")]
    [ProducesResponseType(typeof(IEnumerable<VenueDropdownDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetVenuesForDropdown(CancellationToken cancellationToken)
    {
        var result = await _venueQueryHandler.GetVenuesForDropdown(cancellationToken);
        return HandleResult(result);
    }
}
