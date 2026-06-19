using LiveEvents.Api.Domain.Ports.Events;
using LiveEvents.Api.Events.Application.UseCases.Venues.Dtos;

namespace LiveEvents.Api.Events.Application.UseCases.Venues.Queries;

public class GetVenuesForDropdown
{
    private readonly IVenueRepository _venueRepository;

    public GetVenuesForDropdown(IVenueRepository venueRepository)
    {
        _venueRepository = venueRepository;
    }

    public Task<IEnumerable<VenueDropdownDto>> HandleAsync(CancellationToken cancellationToken)
    {
        IEnumerable<VenueDropdownDto> venues = _venueRepository.QueryActive()
            .OrderBy(v => v.Name)
            .Select(v => new VenueDropdownDto
            {
                Id = v.Id,
                Name = v.Name
            })
            .ToList();

        return Task.FromResult(venues);
    }
}
