using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Events.Application.UseCases.Venues.Dtos;
using LiveEvents.Api.Events.Application.UseCases.Venues.Queries;

namespace LiveEvents.Api.Events.Application.UseCases.Venues.Handlers;

public interface IVenueQueryHandler
{
    Task<Result<IEnumerable<VenueDropdownDto>>> GetVenuesForDropdown(CancellationToken cancellationToken);
}

public class VenueQueryHandler : IVenueQueryHandler
{
    private readonly GetVenuesForDropdown _getVenuesForDropdown;

    public VenueQueryHandler(GetVenuesForDropdown getVenuesForDropdown)
    {
        _getVenuesForDropdown = getVenuesForDropdown;
    }

    public async Task<Result<IEnumerable<VenueDropdownDto>>> GetVenuesForDropdown(CancellationToken cancellationToken)
    {
        try
        {
            var venues = await _getVenuesForDropdown.HandleAsync(cancellationToken);
            return Result.Success(venues);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<VenueDropdownDto>>(Error.Failure("Venue.GetDropdown", ex.Message));
        }
    }
}
