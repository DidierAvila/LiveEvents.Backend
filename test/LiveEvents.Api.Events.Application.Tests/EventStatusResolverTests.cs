using FluentAssertions;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Events.Application.UseCases.Events.Helpers;
using static LiveEvents.Api.Events.Application.Tests.Support.EventsTestData;

namespace LiveEvents.Api.Events.Application.Tests;

public class EventStatusResolverTests
{
    [Fact]
    public void GetCurrentStatus_WhenReferenceDateExceedsEventEndDate_ShouldReturnCompleted_ForRuleRn06()
    {
        var eventEntity = BuildEvent(x =>
        {
            x.Status = EventStatus.Activo;
            x.EndsAt = new DateTime(2026, 1, 10, 18, 0, 0);
        });

        var result = EventStatusResolver.GetCurrentStatus(eventEntity, new DateTime(2026, 1, 10, 18, 1, 0));

        result.Should().Be(EventStatus.Completado);
    }

    [Fact]
    public void GetCurrentStatus_WhenEventIsCancelled_ShouldKeepCancelledStatus()
    {
        var eventEntity = BuildEvent(x => x.Status = EventStatus.Cancelado);

        var result = EventStatusResolver.GetCurrentStatus(eventEntity, DateTime.Now.AddDays(10));

        result.Should().Be(EventStatus.Cancelado);
    }

    [Fact]
    public void GetCurrentStatus_WhenEventHasNotFinished_ShouldRemainActive()
    {
        var eventEntity = BuildEvent(x =>
        {
            x.Status = EventStatus.Activo;
            x.EndsAt = new DateTime(2026, 1, 10, 20, 0, 0);
        });

        var result = EventStatusResolver.GetCurrentStatus(eventEntity, new DateTime(2026, 1, 10, 19, 0, 0));

        result.Should().Be(EventStatus.Activo);
    }
}
