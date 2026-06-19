using FluentAssertions;
using LiveEvents.Api.Domain.Entities.Events;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveEvents.Api.Domain.Tests;

public class EventTests
{
    [Fact]
    public void Should_HaveExpected_Table_Mapping()
    {
        var attribute = typeof(Event).GetCustomAttributes(typeof(TableAttribute), false)
            .Cast<TableAttribute>()
            .SingleOrDefault();

        attribute.Should().NotBeNull();
        attribute!.Name.Should().Be("events");
        attribute.Schema.Should().Be("events");
    }

    [Fact]
    public void Should_Default_Status_To_Activo_And_Initialize_Reservations()
    {
        var entity = CreateValidEvent();

        entity.Status.Should().Be(EventStatus.Activo);
        entity.Reservations.Should().NotBeNull();
        entity.Reservations.Should().BeEmpty();
    }

    [Fact]
    public void Should_Be_Valid_When_Required_Data_Is_Present()
    {
        var entity = CreateValidEvent();

        var validationResults = Validate(entity);

        validationResults.Should().BeEmpty();
    }

    [Fact]
    public void Should_Require_Title_And_Description()
    {
        var entity = CreateValidEvent();
        entity.Title = null!;
        entity.Description = null!;

        var validationResults = Validate(entity);

        validationResults.Should().ContainSingle(x => x.MemberNames.Contains(nameof(Event.Title)));
        validationResults.Should().ContainSingle(x => x.MemberNames.Contains(nameof(Event.Description)));
    }

    [Fact]
    public void Should_Enforce_Max_Lengths_For_Text_Fields()
    {
        var entity = CreateValidEvent();
        entity.Title = new string('T', 101);
        entity.Description = new string('D', 501);

        var validationResults = Validate(entity);

        validationResults.Should().Contain(x => x.MemberNames.Contains(nameof(Event.Title)));
        validationResults.Should().Contain(x => x.MemberNames.Contains(nameof(Event.Description)));
    }

    [Fact]
    public void Should_Configure_TicketPrice_With_Numeric_Precision()
    {
        var attribute = typeof(Event).GetProperty(nameof(Event.TicketPrice))!
            .GetCustomAttributes(typeof(ColumnAttribute), false)
            .Cast<ColumnAttribute>()
            .SingleOrDefault();

        attribute.Should().NotBeNull();
        attribute!.TypeName.Should().Be("numeric(10,2)");
    }

    private static List<ValidationResult> Validate(Event entity)
    {
        var context = new ValidationContext(entity);
        var results = new List<ValidationResult>();

        Validator.TryValidateObject(entity, context, results, validateAllProperties: true);

        return results;
    }

    private static Event CreateValidEvent()
    {
        return new Event
        {
            Id = Guid.NewGuid(),
            Title = "Conferencia de Arquitectura",
            Description = "Evento tecnico para revisar buenas practicas de backend.",
            VenueId = Guid.NewGuid(),
            MaxCapacity = 200,
            StartsAt = DateTime.UtcNow.AddDays(10),
            EndsAt = DateTime.UtcNow.AddDays(10).AddHours(2),
            TicketPrice = 120.50m,
            Type = EventType.Conferencia,
            CreatedAt = DateTime.UtcNow
        };
    }
}
