using LiveEvents.Api.Authentication.Application;
using LiveEvents.Api.Authentication.Extensions;
using LiveEvents.Api.Common.Extensions;
using LiveEvents.Api.Common.Middleware;
using LiveEvents.Api.Common.Transformers;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Si tus controladores están en otro proyecto/librería, añade esto:
builder.Services.AddControllers()
    .AddApplicationPart(typeof(Program).Assembly); // Registra el ensamblado actual

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    // Registramos nuestro transformador de seguridad JWT
    options.AddDocumentTransformer<OpenApiSecurityTransformer>();
});

// Configure extensions Application
builder.Services.AddAuthenticationApplication(builder.Configuration);
builder.Services.AddFrontendCors(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Esto habilita la nueva interfaz gráfica
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Authentication Manager API")
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseGlobalExceptionHandling();
app.UseHttpsRedirection();
app.UseCors(CorsExtensions.AllowFrontendPolicy);
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
