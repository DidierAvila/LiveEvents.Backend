using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;

namespace LiveEvents.Api.Domain.Entities.Authentication;

public enum UserStatus
{
    [PgName("Activo")]
    [Display(Name = "Activo")]
    Activo = 1,

    [PgName("Suspendido")]
    [Display(Name = "Suspendido")]
    Suspendido = 2,

    [PgName("PendienteVerificacion")]
    [Display(Name = "Pendiente de Verificación")]
    PendienteVerificacion = 3
}
