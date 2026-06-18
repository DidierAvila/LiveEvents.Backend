using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace LiveEvents.Api.Common.Transformers;

public class OpenApiSecurityTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        // 1. Inicializar componentes si no existen
        document.Components ??= new OpenApiComponents();

        // Asegurar que el diccionario SecuritySchemes no sea null antes de acceder
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        // 2. Definir el esquema
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: 'Bearer {token}'"
        };

        // 3. Aplicar el requerimiento (usar la referencia específica OpenApiSecuritySchemeReference)
        var securityRequirement = new OpenApiSecurityRequirement
        {
            {
                // El constructor requiere el referenceId; pasamos "Bearer" y el documento anfitrión.
                new OpenApiSecuritySchemeReference("Bearer", document),
                new List<string>()
            }
        };

        // Uso de la propiedad correcta según la definición de OpenApiDocument
        document.Security = new List<OpenApiSecurityRequirement> { securityRequirement };

        return Task.CompletedTask;
    }
}
