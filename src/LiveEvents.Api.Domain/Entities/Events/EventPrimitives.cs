using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;

namespace LiveEvents.Api.Domain.Entities.Events;

public enum EventType
{
    [PgName("Conferencia")]
    [Display(Name = "Conferencia")]
    Conferencia = 1,

    [PgName("Taller")]
    [Display(Name = "Taller")]
    Taller = 2,

    [PgName("Concierto")]
    [Display(Name = "Concierto")]
    Concierto = 3
}

public enum EventStatus
{
    [PgName("Activo")]
    [Display(Name = "Activo")]
    Activo = 1,

    [PgName("Cancelado")]
    [Display(Name = "Cancelado")]
    Cancelado = 2,

    [PgName("Completado")]
    [Display(Name = "Completado")]
    Completado = 3
}

public enum ReservationStatus
{
    [PgName("PendientePago")]
    [Display(Name = "Pendiente Pago")]
    PendientePago = 1,

    [PgName("Confirmada")]
    [Display(Name = "Confirmada")]
    Confirmada = 2,

    [PgName("Cancelada")]
    [Display(Name = "Cancelada")]
    Cancelada = 3,

    [PgName("Perdida")]
    [Display(Name = "Perdida")]
    Perdida = 4
}
