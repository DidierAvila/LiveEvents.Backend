using LiveEvents.Api.Common.Extensions;
using LiveEvents.Api.Common.Middleware;
using LiveEvents.Api.Common.Transformers;
using LiveEvents.Api.Events.Extensions;
using LiveEvents.Api.Events.Application;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddApplicationPart(typeof(Program).Assembly)
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<OpenApiSecurityTransformer>();
});

builder.Services.AddEventsApplication(builder.Configuration);
builder.Services.AddFrontendCors(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Events Manager API")
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });

    app.UseHttpsRedirection();
}

app.UseGlobalExceptionHandling();
app.UseCors(CorsExtensions.AllowFrontendPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
