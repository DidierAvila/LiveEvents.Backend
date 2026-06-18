using System.ComponentModel.DataAnnotations;

namespace LiveEvents.Api.Domain.Entities.Authentication;

public enum UserTypeEnum
{
    [Display(Name = "Consultor")]
    Consultant,
    [Display(Name = "Asistente")]
    Assistant,
    [Display(Name = "Proveedor")]
    Supplier,
    [Display(Name = "Cliente")]
    Client
}
