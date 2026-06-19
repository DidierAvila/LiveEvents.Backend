# LiveEvents.Backend

Backend de la prueba tecnica `LiveEvents`, construido con ASP.NET Core y organizado por modulos para `Authentication`, `Events` y `Notification`.

## Descripcion

El proyecto expone tres APIs independientes:

- `Authentication`: usuarios, login, roles, permisos y sesion.
- `Events`: venues, eventos, reservas y confirmacion de pago.
- `Notification`: notificaciones in-app del usuario autenticado.

La solucion usa una arquitectura por capas con separacion entre dominio, aplicacion, infraestructura y APIs.

## Arquitectura

Estructura principal del repositorio:

```text
src/
  LiveEvents.Api.Authentication/
  LiveEvents.Api.Authentication.Application/
  LiveEvents.Api.Common/
  LiveEvents.Api.Domain/
  LiveEvents.Api.Events/
  LiveEvents.Api.Events.Application/
  LiveEvents.Api.Infrastructure/
  LiveEvents.Api.Notification/
  LiveEvents.Api.Notification.Application/
test/
  LiveEvents.Api.Domain.Tests/
  LiveEvents.Api.Events.Application.Tests/
```

Capas:

- `Domain`: entidades y puertos.
- `Application`: casos de uso, DTOs, validadores y mapeos.
- `Infrastructure`: EF Core, repositorios, `DbContext` y migraciones.
- `Common`: concerns transversales como middleware, CORS, JWT base, errores y helpers.
- `Api`: endpoints, configuracion HTTP y autenticacion por modulo.

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Npgsql
- FluentValidation
- AutoMapper
- Scalar / OpenAPI
- Docker

## Modulos

### Authentication API

Responsabilidades:

- login y emision de JWT
- usuarios y perfil
- roles y permisos
- relaciones rol-permiso y usuario-rol

Proyecto API:

- `src/LiveEvents.Api.Authentication`

### Events API

Responsabilidades:

- consulta y administracion de eventos
- consulta de venues
- creacion, consulta y cancelacion de reservas
- confirmacion de pago protegida por permisos

Proyecto API:

- `src/LiveEvents.Api.Events`

### Notification API

Responsabilidades:

- consulta de notificaciones del usuario autenticado
- conteo de no leidas
- marcar una o todas como leidas

Proyecto API:

- `src/LiveEvents.Api.Notification`

## Requisitos

Antes de ejecutar el proyecto, asegurese de tener instalado:

- .NET SDK 10
- PostgreSQL
- herramienta local `dotnet-ef` restaurada desde `dotnet-tools.json`
- Docker, si desea ejecutar o desplegar con contenedores

Restaurar herramientas:

```bash
dotnet tool restore
```

Restaurar dependencias:

```bash
dotnet restore
```

## Configuracion

Cada API tiene su propio `appsettings.json` y `appsettings.Development.json`.

Variables relevantes:

- `ConnectionStrings__DefaultConnection`
- `JwtSettings__key`
- `JwtSettings__Issuer`
- `JwtSettings__Audience`
- `CorsSettings__AllowedOrigins__0`

Ejemplo de cadena de conexion PostgreSQL:

```text
Host=localhost;Port=5432;Database=liveevents;Username=postgres;Password=admin
```

## Ejecucion local

Puedes ejecutar cada API por separado.

### Authentication

```bash
dotnet run --project .\src\LiveEvents.Api.Authentication\LiveEvents.Api.Authentication.csproj
```

### Events

```bash
dotnet run --project .\src\LiveEvents.Api.Events\LiveEvents.Api.Events.csproj
```

### Notification

```bash
dotnet run --project .\src\LiveEvents.Api.Notification\LiveEvents.Api.Notification.csproj
```

## OpenAPI

En ambiente `Development`, cada API expone:

- documento OpenAPI
- interfaz Scalar

Estas rutas se habilitan solo en desarrollo.

## Autenticacion

El sistema usa JWT Bearer.

Consideraciones actuales:

- `Authentication`, `Events` y `Notification` manejan su propia extension de autenticacion JWT.
- `Common` conserva solo la configuracion base tecnica.
- `Notification` opera sobre el usuario autenticado con endpoints tipo `mine`.
- `ReservationsController` usa permisos explicitos:
  - `reservations.read`
  - `reservations.create`
  - `reservations.cancel`
  - `reservations.confirm_payment`

## Base de datos y migraciones

Las migraciones viven en:

- `src/LiveEvents.Api.Infrastructure/Migrations`

Migraciones relevantes actuales:

- `20260619051004_InitialCreate`
- `20260619053341_SeedAuthenticationData`
- `20260619103000_AddReservationsPermissions`
- `20260619143000_SeedEventsVenues`

### Crear una migracion nueva

```bash
dotnet tool run dotnet-ef -- migrations add NombreMigracion \
  --project .\src\LiveEvents.Api.Infrastructure\LiveEvents.Api.Infrastructure.csproj \
  --startup-project .\src\LiveEvents.Api.Events\LiveEvents.Api.Events.csproj \
  --context LiveEventsDbContext
```

En PowerShell sobre Windows, puedes usar este formato:

```powershell
dotnet tool run dotnet-ef -- migrations add NombreMigracion `
  --project .\src\LiveEvents.Api.Infrastructure\LiveEvents.Api.Infrastructure.csproj `
  --startup-project .\src\LiveEvents.Api.Events\LiveEvents.Api.Events.csproj `
  --context LiveEventsDbContext
```

### Aplicar migraciones en local

```powershell
dotnet tool run dotnet-ef -- database update `
  --project .\src\LiveEvents.Api.Infrastructure\LiveEvents.Api.Infrastructure.csproj `
  --startup-project .\src\LiveEvents.Api.Events\LiveEvents.Api.Events.csproj `
  --context LiveEventsDbContext
```

### Aplicar migraciones en Neon

```powershell
$env:ConnectionStrings__DefaultConnection="Host=TU_HOST;Port=5432;Database=TU_DB;Username=TU_USER;Password=TU_PASSWORD;SSL Mode=Require;Trust Server Certificate=true"

dotnet tool run dotnet-ef -- database update `
  --project .\src\LiveEvents.Api.Infrastructure\LiveEvents.Api.Infrastructure.csproj `
  --startup-project .\src\LiveEvents.Api.Events\LiveEvents.Api.Events.csproj `
  --context LiveEventsDbContext

Remove-Item Env:ConnectionStrings__DefaultConnection
```

### Listar migraciones

```powershell
dotnet tool run dotnet-ef -- migrations list `
  --project .\src\LiveEvents.Api.Infrastructure\LiveEvents.Api.Infrastructure.csproj `
  --startup-project .\src\LiveEvents.Api.Events\LiveEvents.Api.Events.csproj `
  --context LiveEventsDbContext
```

## Datos semilla

Actualmente el proyecto incluye seed por migracion para:

- datos base de autenticacion
- permisos de reservas
- venues iniciales en `events.venues`

Los seeds se manejan con SQL dentro de migraciones para poder versionarlos y aplicarlos en ambientes como Neon sin depender de carga manual.

## Docker

Cada API tiene su propio `Dockerfile`:

- `src/LiveEvents.Api.Authentication/Dockerfile`
- `src/LiveEvents.Api.Events/Dockerfile`
- `src/LiveEvents.Api.Notification/Dockerfile`

Ejemplo de build para `Authentication`:

```bash
docker build -f .\src\LiveEvents.Api.Authentication\Dockerfile -t liveevents-auth .
```

Ejemplo de build para `Events`:

```bash
docker build -f .\src\LiveEvents.Api.Events\Dockerfile -t liveevents-events .
```

Ejemplo de build para `Notification`:

```bash
docker build -f .\src\LiveEvents.Api.Notification\Dockerfile -t liveevents-notification .
```

## Despliegue

Stack recomendado para demo:

- Render para APIs
- Neon para PostgreSQL

Puntos importantes del despliegue:

- usar `Docker` como runtime en Render
- dejar `Root Directory` vacio
- configurar `Dockerfile Path` segun la API
- cargar variables de entorno de conexion, JWT y CORS
- crear la base en Neon antes de desplegar
- ejecutar migraciones contra Neon antes de probar el frontend

Frontend configurado en produccion:

- `https://liveeventsplatform.netlify.app`

## Pruebas

Ejecutar todas las pruebas:

```bash
dotnet test
```

Ejecutar solo las pruebas de eventos:

```bash
dotnet test .\test\LiveEvents.Api.Events.Application.Tests\LiveEvents.Api.Events.Application.Tests.csproj
```

## Seguridad y mejoras aplicadas

Se incorporaron varias mejoras durante la evolucion del proyecto:

- endurecimiento de `Notification` para operar solo sobre el usuario autenticado
- activacion de `UseAuthentication()` en `Notification`
- proteccion por permisos en reservas
- reduccion de exposicion de mensajes internos de excepcion
- alineacion del mapeo `authentication.user_status`
- separacion de `JwtAuthenticationExtensions` por API

## Archivo de solucion

La solucion principal del repositorio es:

- `LiveEvents.Api.slnx`

## Notas

- Scalar y OpenAPI se exponen solo en `Development`.
- En Render, `UseHttpsRedirection()` se deja solo para desarrollo para evitar conflictos dentro del contenedor.
- Cuando agregues nuevos permisos, los usuarios deben iniciar sesion de nuevo para recibirlos en el JWT.
