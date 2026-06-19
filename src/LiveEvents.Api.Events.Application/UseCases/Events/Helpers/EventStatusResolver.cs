using LiveEvents.Api.Domain.Entities.Events;

namespace LiveEvents.Api.Events.Application.UseCases.Events.Helpers;

public static class EventStatusResolver
{
    public static EventStatus GetCurrentStatus(Event eventEntity, DateTime? referenceDate = null)
    {
        var now = referenceDate ?? DateTime.Now;

        if (eventEntity.Status == EventStatus.Cancelado)
        {
            return EventStatus.Cancelado;
        }

        return now > eventEntity.EndsAt
            ? EventStatus.Completado
            : EventStatus.Activo;
    }
}
