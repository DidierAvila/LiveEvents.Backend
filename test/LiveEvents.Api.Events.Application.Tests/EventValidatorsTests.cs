using FluentAssertions;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Events.Application.UseCases.Events.Dtos;
using LiveEvents.Api.Events.Application.UseCases.Events.Validators;
using static LiveEvents.Api.Events.Application.Tests.Support.EventsTestData;

namespace LiveEvents.Api.Events.Application.Tests;

public class EventValidatorsTests
{
    [Fact]
    public void CreateEventDtoValidator_WhenTypeIsDefault_ShouldReturnValidationError()
    {
        var validator = new CreateEventDtoValidator();
        var dto = BuildCreateEventDto(x => x.Type = 0);

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(CreateEventDto.Type));
    }

    [Fact]
    public void EventFilterDtoValidator_WhenTypeIsInvalid_ShouldReturnValidationError()
    {
        var validator = new EventFilterDtoValidator();
        var dto = new EventFilterDto
        {
            Type = (EventType)99
        };

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(EventFilterDto.Type));
    }

    [Fact]
    public void EventFilterDtoValidator_WhenStatusIsInvalid_ShouldReturnValidationError()
    {
        var validator = new EventFilterDtoValidator();
        var dto = new EventFilterDto
        {
            Status = (EventStatus)99
        };

        var result = validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(EventFilterDto.Status));
    }
}
